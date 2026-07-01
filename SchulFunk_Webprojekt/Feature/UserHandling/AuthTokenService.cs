namespace SchulFunk_Webprojekt.Feature.UserHandling;

public class AuthTokenService
{
    public string? Token { get; private set; }

    public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

    // NEU: Event für Statusänderungen
    public event Action? OnChange;

    public void SetToken(string token)
    {
        Token = token;
        OnChange?.Invoke(); // NEU: Event auslösen
    }

    public void ClearToken()
    {
        Token = null;
        OnChange?.Invoke(); // NEU: Event auslösen
    }
}