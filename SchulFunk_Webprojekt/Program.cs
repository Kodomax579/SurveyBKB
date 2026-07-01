using Microsoft.AspNetCore.Components.Authorization;
using SchulFunk_Webprojekt.Components;
using SchulFunk_Webprojekt.Feature.NewsHandling;
using SchulFunk_Webprojekt.Feature.SurveyHandling;
using SchulFunk_Webprojekt.Feature.UserHandling;
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

            builder.Services.AddScoped<AuthTokenService>();
            builder.Services.AddScoped<SignalRHub.SignalRHub>();

            builder.Services.AddScoped<NewsStateService>();
            builder.Services.AddScoped<UserStateService>();
            builder.Services.AddScoped<SurveyStateService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            // 3. Das Autorisierungs-System von Blazor aktivieren
            // Configure typed HttpClients with the API base URL from configuration
            var apiBase = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl");
            if (!string.IsNullOrWhiteSpace(apiBase))
            {
                builder.Services.AddHttpClient<NewsService>(client => client.BaseAddress = new Uri(apiBase));
                builder.Services.AddHttpClient<SurveyService>(client => client.BaseAddress = new Uri(apiBase));
                builder.Services.AddHttpClient<UserService>(client => client.BaseAddress = new Uri(apiBase));
            }
            else
            {
                // Fallback to default registration if no base URL is configured
                builder.Services.AddHttpClient<NewsService>();
                builder.Services.AddHttpClient<SurveyService>();
                builder.Services.AddHttpClient<UserService>();
            }
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        // Wenn das System merkt, dass jemand nicht eingeloggt ist, 
        // wird er automatisch direkt hierhin weitergeleitet:
        options.LoginPath = "/";
    });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
