using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SchulFunk_Webprojekt.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthTokenService _authTokenService;
    private const string BaseUrl = "http://212.227.82.199:7224";

    public ApiService(HttpClient httpClient, AuthTokenService authTokenService)
    {
        _httpClient = httpClient;
        _authTokenService = authTokenService;
    }

    private void AddToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrWhiteSpace(_authTokenService.Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authTokenService.Token);
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        AddToken();
        return await _httpClient.GetAsync($"{BaseUrl}{url}");
    }

    public async Task<T?> GetFromJsonAsync<T>(string url)
    {
        AddToken();
        var result = await _httpClient.GetFromJsonAsync<T>($"{BaseUrl}{url}");
        return result;
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T data)
    {
        AddToken();
        return await _httpClient.PostAsJsonAsync($"{BaseUrl}{url}", data);
    }

    public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T data)
    {
        AddToken();
        return await _httpClient.PutAsJsonAsync($"{BaseUrl}{url}", data);
    }

    public async Task<HttpResponseMessage> PutAsync(string url)
    {
        AddToken();
        return await _httpClient.PutAsync($"{BaseUrl}{url}", null);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        AddToken();
        return await _httpClient.DeleteAsync($"{BaseUrl}{url}");
    }
}