using SchulFunk_Webprojekt.Model;

namespace SchulFunk_Webprojekt.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthTokenService _authTokenService;
        private const string BaseUrl = "http://212.227.82.199:7224";
        public UserModel CurrentUser { get; set; }
        public UserService(HttpClient httpClient, AuthTokenService authTokenService)
        {
            _httpClient = httpClient;
            _authTokenService = authTokenService;
        }

        public async Task<bool> Login(string email, string password)
        {
            var loginRequest = new { Email = email, Password = password };

            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/api/login", loginRequest);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var loginResult = await response.Content.ReadFromJsonAsync<LoginModel>();

            if (loginResult == null || string.IsNullOrWhiteSpace(loginResult.Token))
            {
                return false;
            }

            CurrentUser = loginResult.User;
            _authTokenService.SetToken(loginResult.Token);

            return true;
        }
    }
}
