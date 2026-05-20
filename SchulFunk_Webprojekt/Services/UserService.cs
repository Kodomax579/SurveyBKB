namespace SchulFunk_Webprojekt.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthTokenService _authTokenService;

        public UserService(HttpClient httpClient, AuthTokenService authTokenService)
        {
            _httpClient = httpClient;
            _authTokenService = authTokenService;
        }

        public async Task<bool> Login(string email, string password)
        {
            var url =
                $"/api/Login?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            if (!response.Headers.TryGetValues("Authorization", out var tokenValues))
            {
                return false;
            }

            var token = tokenValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            _authTokenService.SetToken(token);

            return true;
        }
    }
}
