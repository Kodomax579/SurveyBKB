namespace SchulFunk_Webprojekt.Services;

public class AuthTokenService
{
    public string? Token { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

    public void SetToken(string token)
    {
        Token = token;
    }

    public void ClearToken()
    {
        Token = null;
    }
}