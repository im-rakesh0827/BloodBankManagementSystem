using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace BloodBankManagementSystem.UI.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        // public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        // {
        //     var savedToken = await _localStorage.GetItemAsStringAsync("authToken");

        //     if (string.IsNullOrWhiteSpace(savedToken))
        //     {
        //         return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        //     }

        //     var identity = new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt");
        //     var user = new ClaimsPrincipal(identity);

        //     return new AuthenticationState(user);
        // }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
{
    var token = await _localStorage.GetItemAsync<string>("authToken"); // Make sure the key matches
    var identity = new ClaimsIdentity();
    
    if (!string.IsNullOrEmpty(token) && IsValidJwt(token))
    {
        identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
    }
    
    var user = new ClaimsPrincipal(identity);
    return new AuthenticationState(user);
}

// ✅ Helper method to validate JWT format
private bool IsValidJwt(string token)
{
    var parts = token.Split('.');
    return parts.Length == 3; // JWT must have three parts: Header.Payload.Signature
}


        public void MarkUserAsAuthenticated(string token)
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }

        // private IEnumerable<Claim> ParseClaimsFromJwt(string token)
        // {
        //     var handler = new JwtSecurityTokenHandler();
        //     var jwtToken = handler.ReadJwtToken(token);

        //     return jwtToken.Claims;
        // }

        private IEnumerable<Claim> ParseClaimsFromJwt(string token)
{
    if (string.IsNullOrWhiteSpace(token) || !IsValidJwt(token))
    {
        return new List<Claim>(); // Avoid exceptions due to invalid tokens
    }

    var handler = new JwtSecurityTokenHandler();
    var jwt = handler.ReadJwtToken(token);
    return jwt.Claims;
}

    }
}
