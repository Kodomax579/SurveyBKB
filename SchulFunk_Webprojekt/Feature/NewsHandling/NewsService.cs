using SchulFunk_Webprojekt.Feature.NewsHandling.Model;
using SchulFunk_Webprojekt.Feature.UserHandling;
using System.Net.Http.Headers;
using System.Net.Http.Json; 

namespace SchulFunk_Webprojekt.Feature.NewsHandling
{
    public class NewsService(IConfiguration configuration, HttpClient httpClient, NewsStateService newsStateService, AuthTokenService authTokenService)
    {
        private readonly string? BaseURL = configuration.GetValue<string>("ApiSettings:BaseUrl");


        private void AddToken()
        {
            httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(authTokenService.Token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authTokenService.Token);
            }
        }
        public async Task GetAllNews()
        {
            AddToken();
            var result = await httpClient.GetAsync($"{BaseURL}/api/News");

            if (!result.IsSuccessStatusCode) return;

            var newsList = await result.Content.ReadFromJsonAsync<List<NewsItem>>();
            if (newsList != null)
            {
                newsStateService.SetNews(newsList);
            }
        }

        public async Task<NewsItem?> GetNewsById(int id)
        {
            AddToken();
            var result = await httpClient.GetAsync($"{BaseURL}/api/News/{id}");

            if (!result.IsSuccessStatusCode) return null;

            return await result.Content.ReadFromJsonAsync<NewsItem>();
        }

        public async Task<bool> CreateNews(NewsItem newNews)
        {
            AddToken();
            var result = await httpClient.PostAsJsonAsync($"{BaseURL}/api/News", newNews);

            if (!result.IsSuccessStatusCode) return false;
            
            var content = await result.Content.ReadFromJsonAsync<NewsItem>();

            newsStateService.AddNewsItem(content);

            return true;
        }

        public async Task<bool> UpdateNews(int id, NewsItem updatedNews)
        {
            AddToken();
            var result = await httpClient.PutAsJsonAsync($"{BaseURL}/api/News/{id}", updatedNews);

            if (result.IsSuccessStatusCode)
            {
                newsStateService.UpdateNewsItem(updatedNews);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteNews(int id)
        {
            AddToken();
            var result = await httpClient.DeleteAsync($"{BaseURL}/api/News/{id}");

            if (result.IsSuccessStatusCode)
            {
                newsStateService.DeleteNewsItem(id);
                return true;
            }
            return false;
        }
    }
}