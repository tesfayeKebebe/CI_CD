using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Constants;
using RazorShared.Server.Extensions;
using RazorShared.Server.Models.Security;
using RazorShared.StateManagement;

namespace RazorShared.Server.Common.Services;

public class RefreshTokenService
{
    private  AuthenticationStateService _authProvider;
    private readonly IAuthenticationService _authService;
    private readonly SessionStoreService _sessionStoreService;
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly BearerAuthStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;
    private readonly NavigationStateService _navigationStateService;

    public RefreshTokenService(AuthenticationStateService authProvider, IAuthenticationService authService, SessionStoreService sessionStoreService, ApiSetting apiSetting, HttpClient httpClient, BearerAuthStateProvider authStateProvider, NavigationManager navigationManager, NavigationStateService navigationStateService)
    {
        _authProvider = authProvider;
        _authService = authService;
        _sessionStoreService = sessionStoreService;
        _apiSetting = apiSetting;
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _navigationManager = navigationManager;
        _navigationStateService = navigationStateService;
    }

    public async Task<AuthenticationViewModel?> TryRefreshToken()
    {
        var session = await _sessionStoreService.Get();
        var user = _authProvider.TokenQueries;
        if (session.Authentication == null)
        {
            return	await	_authService.RefreshToken(user);
        }

        var refreshQuery = new RefreshTokenQuery
        {
            RefreshToken = session.Authentication.RefreshToken,
            Username = session.Authentication.Username
        };
        _authProvider.SetAuthentication(refreshQuery);
        return session?.Authentication;

     ;
    }

    
}