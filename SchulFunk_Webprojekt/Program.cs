using SchulFunk_Webprojekt.Components;
using SchulFunk_Webprojekt.Services;
using SchulFunk_Webprojekt.SignalRHub;

namespace SchulFunk_Webprojekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddSingleton<AuthTokenService>();
            builder.Services.AddSingleton<SignalRHub.SignalRHub>();
            builder.Services.AddScoped<ApiService>();

            builder.Services.AddScoped(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["ApiSettings:BaseUrl"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new InvalidOperationException("ApiSettings:BaseUrl fehlt in appsettings.json.");
                }

                return new HttpClient
                {
                    BaseAddress = new Uri(baseUrl)
                };
            });

            builder.Services.AddScoped<UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapHub<SignalRHub.SignalRHub>("/realtimehub");

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
