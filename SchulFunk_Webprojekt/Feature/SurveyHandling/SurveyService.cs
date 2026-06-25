using SchulFunk_Webprojekt.Feature.SurveyHandling.Model;
using SchulFunk_Webprojekt.Feature.UserHandling;
using System.Net.Http.Headers;
using System.Net.Http.Json; 

namespace SchulFunk_Webprojekt.Feature.SurveyHandling
{
    public class SurveyService(IConfiguration configuration, HttpClient httpClient, SurveyStateService surveyStateService, AuthTokenService authTokenService)
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
        public async Task GetAllSurveys()
        {
            AddToken();
            var result = await httpClient.GetAsync($"{BaseURL}/api/Survey");

            if (!result.IsSuccessStatusCode) return;

            var surveys = await result.Content.ReadFromJsonAsync<List<SurveyModel>>();
            if (surveys != null)
            {
                surveyStateService.SetSurvey(surveys);
            }
        }

        public async Task<SurveyModel?> GetSurveyById(int id)
        {
            AddToken();
            var result = await httpClient.GetAsync($"{BaseURL}/api/Survey/{id}");

            if (!result.IsSuccessStatusCode) return null;

            return await result.Content.ReadFromJsonAsync<SurveyModel>();
        }

        public async Task<bool> CreateSurvey(SurveyModel newSurvey)
        {
            AddToken();
            var result = await httpClient.PostAsJsonAsync($"{BaseURL}/api/Survey", newSurvey);

            if (!result.IsSuccessStatusCode) return false;
            
            return true;
        }

        public async Task<bool> DeleteSurvey(int id)
        {
            AddToken();
            var result = await httpClient.DeleteAsync($"{BaseURL}/api/Survey/{id}");

            if (result.IsSuccessStatusCode)
            {
                surveyStateService.DeleteSurveyItem(id);
                return true;
            }
            return false;
        }

        public async Task<bool> EndSurveyEarly(int id)
        {
            AddToken();

            var result = await httpClient.PutAsync($"{BaseURL}/api/Survey/{id}/end", null);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> VoteForAnswers(List<int> answerIds, int userId)
        {
            AddToken();

            var result = await httpClient.PutAsJsonAsync($"{BaseURL}/api/Survey/vote?userId={userId}", answerIds);
            return result.IsSuccessStatusCode;
        }
    }
}