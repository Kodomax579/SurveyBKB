using Contracts.Protos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Survey.ApiGateway.RealtimeHub;
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
            builder.Services.AddSignalR();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("Authorization")
                          .AllowCredentials();
                });
            });

            var secretKey = "DEIN_SEHR_LANGER_GEHEIMER_SCHLUESSEL_123!";
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            // 2. Authentifizierung hinzufügen
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, 
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes), 
                    ValidateIssuer = false,
                    ValidateAudience = false, 
                    ValidateLifetime = true, 
                    ClockSkew = TimeSpan.Zero 
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/realtimehub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
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

            app.UseAuthentication(); 
            app.UseAuthorization();  
            app.MapHub<RealtimeHub.RealtimeHub>("/realtimehub");
            app.MapControllers();
            app.Run();
        }
    }
}
