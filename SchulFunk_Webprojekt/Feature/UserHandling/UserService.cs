using SchulFunk_Webprojekt.Feature.UserHandling.Model;
using System.Net.Http.Headers;

namespace SchulFunk_Webprojekt.Feature.UserHandling
{
    public class UserService(HttpClient httpClient, AuthTokenService authTokenService, UserStateService userStateService, IConfiguration configuration)
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

        public async Task<bool> Login(string email, string password)
        {
            var loginRequest = new { Email = email, Password = password };
            var response = await httpClient.PostAsJsonAsync($"{BaseURL}/api/login", loginRequest);
            if (!response.IsSuccessStatusCode) return false;

            var loginResult = await response.Content.ReadFromJsonAsync<LoginModel>();

            if (loginResult == null || string.IsNullOrWhiteSpace(loginResult.Token))
                return false;

            userStateService.SetUser(loginResult.User);
            authTokenService.SetToken(loginResult.Token);

            return true;
        }

        public async Task<List<UserModel>?> GetAllUsers()
        {
            AddToken();
            var response = await httpClient.GetAsync($"{BaseURL}/api/User");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<UserModel>>();
        }

        public async Task<bool> CreateUser(UserModel newUser)
        {
            AddToken();
            var response = await httpClient.PostAsJsonAsync($"{BaseURL}/api/User", newUser);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUser(int id, UserModel updatedUser)
        {
            AddToken();
            var response = await httpClient.PutAsJsonAsync($"{BaseURL}/api/User/{id}", updatedUser);

            if (response.IsSuccessStatusCode)
            {
                var currentUser = userStateService.GetUser();
                if (currentUser != null && currentUser.Id == id)
                {
                    userStateService.UpdateUser(updatedUser);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUser(int id)
        {
            AddToken();
            var response = await httpClient.DeleteAsync($"{BaseURL}/api/User/{id}");

            if (response.IsSuccessStatusCode)
            {
                var currentUser = userStateService.GetUser();
                if (currentUser != null && currentUser.Id == id)
                {
                    userStateService.ClearUser();
                }
                return true;
            }
            return false;
        }

        public async Task<List<ClassModel>?> GetAllClasses()
        {
            AddToken();
            var response = await httpClient.GetAsync($"{BaseURL}/api/User/classes");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<ClassModel>>();
        }

        public async Task<bool> CreateClass(ClassModel newClass)
        {
            AddToken();
            var response = await httpClient.PostAsJsonAsync($"{BaseURL}/api/User/classes", newClass);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            AddToken();

            ChangePasswordDTO dto = new ChangePasswordDTO()
            {
                NewPassword = newPassword,
                CurrentPassword = oldPassword,
            };

            var response = await httpClient.PutAsJsonAsync($"{BaseURL}/api/User/ChangePassword/{userId}", dto);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine(content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}