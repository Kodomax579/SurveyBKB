using Contracts.Protos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Survey.ApiGateway
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region --- Mapster configuration ---
            TypeAdapterConfig<Timestamp, DateTime>.NewConfig()
                .MapWith(ts => ts.ToDateTime());

            TypeAdapterConfig<DateTime, Timestamp>.NewConfig()
                .MapWith(dt => Timestamp.FromDateTime(dt.ToUniversalTime()));

            TypeAdapterConfig<ByteString, byte[]>.NewConfig()
                .MapWith(bs => bs.ToByteArray());

            TypeAdapterConfig<byte[], ByteString>.NewConfig()
                .MapWith(b => ByteString.CopyFrom(b ?? Array.Empty<byte>()));

            TypeAdapterConfig<DateOnly, Timestamp>.NewConfig()
                .MapWith(d => Timestamp.FromDateTime(d.ToDateTime(TimeOnly.MinValue).ToUniversalTime()));

            TypeAdapterConfig<Timestamp, DateOnly>.NewConfig()
                .MapWith(ts => DateOnly.FromDateTime(ts.ToDateTime().ToLocalTime()));
            #endregion

            // Add user grpc connection.
            builder.Services.AddGrpcClient<User.UserClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:UserServiceUrl"]);
            });

            //Add news grpc connection
            builder.Services.AddGrpcClient<News.NewsClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:NewsServiceUrl"]);
            });

            builder.Services.AddGrpcClient<Contracts.Protos.Survey.SurveyClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:SurveyServiceUrl"]);
            });

            builder.Services.AddScoped<Services.AuthService>();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("Authorization");
                });
            });

            var secretKey = "DEIN_SEHR_LANGER_GEHEIMER_SCHLUESSEL_123!";
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            // 2. Authentifizierung hinzufügen
            builder.Services.AddAuthentication(options =>
            {
                // Sag ASP.NET, dass wir standardmäßig JWTs im "Authorization" Header nutzen
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Hier legen wir die Regeln fest, wann ein Token gültig ist
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Prüfe die Unterschrift
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes), // Mit diesem Schlüssel
                    ValidateIssuer = false, // (Auf true setzen, wenn du Issuer streng prüfen willst)
                    ValidateAudience = false, // (Auf true setzen, wenn du Audience streng prüfen willst)
                    ValidateLifetime = true, // Prüfe, ob der Token schon abgelaufen ist
                    ClockSkew = TimeSpan.Zero // Keine Extra-Kulanzzeit beim Ablaufdatum
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication(); // 1. "Wer bist du?" -> Liest den Token aus dem Header und entschlüsselt ihn.
            app.UseAuthorization();  // 2. "Darfst du das?" -> Prüft, ob das [Authorize] Attribut erlaubt wird.

            app.MapControllers();
            app.Run();
        }
    }
}
