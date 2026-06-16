using Microsoft.AspNetCore.Components.Forms;
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

        public async Task<bool> CreateNews(NewsItem newNews, IBrowserFile? imageFile = null)
        {
            AddToken();

            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(newNews.Id.ToString()), "Id");
            content.Add(new StringContent(newNews.Title ?? ""), "Titel");
            content.Add(new StringContent(newNews.Tag ?? ""), "Tag");
            content.Add(new StringContent(newNews.PreviewText ?? ""), "PreviewText");
            content.Add(new StringContent(newNews.MainText ?? ""), "MainText");
            content.Add(new StringContent(newNews.Prioritaet ?? ""), "Prioritaet");
            content.Add(new StringContent(newNews.CreatedAt.ToString("yyyy-MM-dd")), "CreatedAt");
            content.Add(new StringContent(newNews.ExpiredDate.ToString("yyyy-MM-dd")), "ExpiredDate");
            content.Add(new StringContent(newNews.NumberOfMembers.ToString()), "NumberOfMembers");

            if (newNews.UserModel != null)
            {
                content.Add(new StringContent(newNews.UserModel.Id.ToString()), "User.Id");
            }
            // Stelle sicher, dass die Datei in ein Byte-Array gelesen wird, bevor sie dem Multipart-Content hinzugefügt wird.
            // Das vermeidet Probleme mit nicht-seekbaren Streams in Blazor-Umgebungen.
            if (imageFile != null)
            {
                using var fileStream = imageFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var byteContent = new ByteArrayContent(fileBytes);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);

                content.Add(byteContent, "image", imageFile.Name);
            }

            var result = await httpClient.PostAsync($"{BaseURL}/api/News", content);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponse = await result.Content.ReadAsStringAsync();
                Console.WriteLine($"Fehler beim Speichern: {errorResponse}");
                return false;
            }

            var createdNews = await result.Content.ReadFromJsonAsync<NewsItem>();

            if (createdNews == null)
            {
                return false;
            }

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