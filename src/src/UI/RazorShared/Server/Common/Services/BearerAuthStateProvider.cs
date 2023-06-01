using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using RazorShared.Server.Constants;

namespace RazorShared.Server.Common.Services
{
    public class BearerAuthStateProvider : AuthenticationStateProvider
    {
        private readonly SessionStoreService _sessionDataService;
        private readonly AuthenticationState _anonymousUser = new(new ClaimsPrincipal());

        public BearerAuthStateProvider(SessionStoreService sessionDataService)
        {
            _sessionDataService = sessionDataService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sessionData = await _sessionDataService.Get();
            if (sessionData.Authentication == null)
            {
                return _anonymousUser;
            }

            var identity = new List<Claim>
        {
            new(ClaimTypes.Name, sessionData?.Authentication?.Username!),
            new("refresh_token", sessionData?.Authentication?.RefreshToken!),
            new("token_type", sessionData?.Authentication?.Type!),
            new("expires_in", sessionData?.Authentication?.ExpiresIn.ToString(CultureInfo.InvariantCulture)!),
            new("expires_at", sessionData?.Authentication?.ExpiresAt.ToString(CultureInfo.InvariantCulture)!),
         
        };
            var claims = new List<Claim>();
            
            if (sessionData?.Authentication?.Roles is { } list)
            {
                if (sessionData?.Authentication?.Roles != null)
                {
                    foreach (var role in sessionData?.Authentication?.Roles!)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
            }
            claims.AddRange(identity);
            claims.AddRange(claims);
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims,"api"));
            var authenticationState = new AuthenticationState(principal);
            return authenticationState;
        }

        public async Task MarkUserAsAuthenticated(string? username)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(await GetClaims(username)
                , AuthorizationSchemes.Bearer));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
        private async Task< IEnumerable<Claim>> GetClaims(string? username)
        {
            var claims = new List<Claim>();
            var sessionData =await _sessionDataService.Get();
            claims.Add(new Claim(ClaimTypes.Name, username!));
            if (sessionData?.Authentication?.Roles is { } list)
            {
                claims.AddRange(from role in sessionData?.Authentication?.Roles select new Claim(ClaimTypes.Role, role));
            }
            claims.AddRange(claims);
            return claims;
        }
        public void MarkUserLoggedOut()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymousUser));
        }
    }

}
