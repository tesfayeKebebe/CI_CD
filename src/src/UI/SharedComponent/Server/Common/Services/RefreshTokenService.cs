using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.Security;
using SharedComponent.StateManagement;

namespace SharedComponent.Server.Common.Services;

public class RefreshTokenService
{
    private  AuthenticationStateService _authProvider;
    private readonly IAuthenticationService _authService;
    private readonly SessionStoreService _sessionStoreService;

    private readonly HttpClient _httpClient;
    private readonly BearerAuthStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;
    private readonly NavigationStateService _navigationStateService;

    public RefreshTokenService(AuthenticationStateService authProvider, IAuthenticationService authService, SessionStoreService sessionStoreService,  HttpClient httpClient, BearerAuthStateProvider authStateProvider, NavigationManager navigationManager, NavigationStateService navigationStateService)
    {
        _authProvider = authProvider;
        _authService = authService;
        _sessionStoreService = sessionStoreService;
       
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _navigationManager = navigationManager;
        _navigationStateService = navigationStateService;
    }

    public async Task<AuthenticationViewModel?> TryRefreshToken()
    {
        var session = await _sessionStoreService.Get();
        var user = _authProvider.TokenQueries;
        if (session.Authentication != null && session.Authentication.ExpiresAt < DateTime.Now)
        {
            await _sessionStoreService.RemoveAsync();
            return null;
        }
        var query = new RefreshTokenQuery
        {
            RefreshToken = session.Authentication?.RefreshToken,
            Username = session.Authentication?.Username
        };
        var refresh= await _authService.RefreshToken(query);
        if (refresh == null)
        {
            return refresh;
        }

        var sessionStore = new SessionStore
        {
            Authentication = refresh
        };
        await   _sessionStoreService.Set(sessionStore);
        await _authStateProvider.MarkUserAsAuthenticated(refresh?.Username);
        return refresh;

    }

    
}