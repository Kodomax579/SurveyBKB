using NewsService.Database;
using NewsService.Services;

namespace NewsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddScoped<Feature.NewsService>();
            builder.Services.AddSqlite<NewsContext>("Data Source=NewsDatabase.db");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<GrpcsNewsService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
