using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using SchulFunk_Webprojekt.Feature.NewsHandling.Model;
using SchulFunk_Webprojekt.Feature.UserHandling;

namespace SchulFunk_Webprojekt.SignalRHub
{
    public class SignalRHub(AuthTokenService authTokenService) : Hub
    {
        private HubConnection? _hubConnection;

        public event Action<NewsItem>? OnNewsItemsUpdated;

        public async Task Connect()
        {
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected)
            {
                return;
            }

            if (authTokenService.Token is null)
            {
                return;
            }

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://212.227.82.199:7224/realtimeHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(authTokenService.Token);
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<NewsItem>("ReceiveNewNews", newsItems =>
            {
                OnNewsItemsUpdated?.Invoke(newsItems);
            });
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to SignalR hub: {ex.Message}");
            }
        }
    }
}
