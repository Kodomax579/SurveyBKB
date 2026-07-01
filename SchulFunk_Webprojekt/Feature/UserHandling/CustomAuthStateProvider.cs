using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchulFunk_Webprojekt.Feature.UserHandling;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthTokenService _authTokenService;
    private readonly UserStateService _userStateService;

    public CustomAuthStateProvider(AuthTokenService authTokenService, UserStateService userStateService)
    {
        _authTokenService = authTokenService;
        _userStateService = userStateService;

        // Blazor benachrichtigen, wenn der Token sich ändert (Login / Logout)
        _authTokenService.OnChange += UpdateAuthState;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = _authTokenService.Token;
        var user = _userStateService.GetUser();

        // Wenn kein Token oder kein User da ist -> Nicht eingeloggt
        if (string.IsNullOrWhiteSpace(token) || user == null)
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        // Wenn eingeloggt: Blazor-Identität aus deinem UserModel aufbauen
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email ?? "User"), 
            
            // WICHTIG: Falls dein UserModel eine Eigenschaft für Rollen hat (z.B. "Admin", "Teacher"),
            // füge sie hier hinzu, damit das [Authorize(Roles = "Admin")] Attribut funktioniert:
            // new Claim(ClaimTypes.Role, user.Role ?? "Student") 
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    private void UpdateAuthState()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}