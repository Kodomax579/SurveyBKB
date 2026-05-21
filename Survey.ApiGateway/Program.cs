using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using Survey.ApiGateway.Database;
using Survey.ApiGateway.RealtimeHub;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Services;
using Survey.ApiGateway.Feature.User;
using Survey.ApiGateway.Feature.News;
using Survey.ApiGateway.Feature.Survey;

namespace Survey.ApiGateway
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

#if DEBUG
            builder.Services.AddDbContext<SurveyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionLocal")));
#else
            builder.Services.AddDbContext<SurveyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionPublish")));
#endif

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<NewsService>();
            builder.Services.AddScoped<SurveyService>();
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
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseAuthentication(); 
            app.UseAuthorization();  
            app.MapHub<RealtimeHub.RealtimeHub>("/realtimehub");
            app.MapControllers();
            app.Run();
        }
    }
}
