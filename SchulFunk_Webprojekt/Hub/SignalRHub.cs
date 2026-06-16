using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using SchulFunk_Webprojekt.Feature.NewsHandling.Model;
using SchulFunk_Webprojekt.Feature.SurveyHandling.Model;
using SchulFunk_Webprojekt.Feature.UserHandling;

namespace SchulFunk_Webprojekt.SignalRHub
{
    public class SignalRHub(AuthTokenService authTokenService) : Hub
    {
        private HubConnection? _hubConnection;

        public event Action<NewsItem>? OnNewsItemsUpdated;
        public event Action<NewsItem>? OnNewsItemCreated;
        public event Action<int>? OnNewsItemDeleted;
        public event Action<SurveyModel>? OnSurveyCreated;
        public event Action<int>? OnSurveyDeleted;
        public event Action<SurveyModel>? OnSurveyVoteUpdate;

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
                .WithUrl("http://212.227.82.199:7189/realtimeHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(authTokenService.Token);
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<NewsItem>("NewsUpdated", newsItems =>
            {
                OnNewsItemsUpdated?.Invoke(newsItems);
            });
            _hubConnection.On<NewsItem>("NewsCreated", newsItems =>
            {
                OnNewsItemCreated?.Invoke(newsItems);
            });
            _hubConnection.On<int>("NewsDeleted", newsId =>
            {
                OnNewsItemDeleted?.Invoke(newsId);
            });
            _hubConnection.On<SurveyModel>("ReceiveNewSurvey", newSurvey =>
            {
                OnSurveyCreated?.Invoke(newSurvey);
            });
            _hubConnection.On<SurveyModel>("SurveyVoteUpdated", survey =>
            {
                OnSurveyVoteUpdate?.Invoke(survey);
            });
            _hubConnection.On<int>("SurveyDeleted", surveyId =>
            {
                OnSurveyDeleted?.Invoke(surveyId);
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
