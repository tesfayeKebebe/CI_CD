using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Constants;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.Security;
using Web.UI.StateManagement;

namespace Web.UI.Server.Common.Services;

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