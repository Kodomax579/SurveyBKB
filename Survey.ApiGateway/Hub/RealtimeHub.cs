using Microsoft.AspNetCore.SignalR;
using Survey.ApiGateway.Models;

namespace Survey.ApiGateway.RealtimeHub
{
    public class RealtimeHub : Hub
    {
        public async Task SendNewNews(List<NewsModel> news)
        {
            await Clients.All.SendAsync("NewNews", news);
        }

        public async Task SendNewSurvey(List<SurveyModel> survey)
        {
            await Clients.All.SendAsync("NewSurvey", survey);
        }
    }
}
