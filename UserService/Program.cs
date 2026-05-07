using UserService.Database;
using UserService.Feature;
using UserService.Service;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddSqlite<UserContext>("Data Source=UserDatabase.db");
            builder.Services.AddScoped<Feature.UserService>();
            builder.Services.AddScoped<Feature.ClassService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserGrpcService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
