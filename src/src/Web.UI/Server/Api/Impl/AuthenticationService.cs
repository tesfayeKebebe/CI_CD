using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Constants;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.LabTests;
using Web.UI.Server.Models.Security;
using Web.UI.StateManagement;

namespace Web.UI.Server.Api.Impl;

public class AuthenticationService : IAuthenticationService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStoreService;
    private readonly BearerAuthStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;
    public AuthenticationService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStoreService, 
        BearerAuthStateProvider authStateProvider, NavigationManager navigationManager)
    {
        _apiSetting = apiSetting;
        _httpClient = httpClient;
        _sessionStoreService = sessionStoreService;
        _authStateProvider = authStateProvider;
        _navigationManager = navigationManager;
    }

    public async Task<AuthenticationViewModel?> Login(LoginModel model)
    {
        StringBuilder buld = new StringBuilder();
        buld.Append(_apiSetting.BaseUrl).Append(ApiPath.LoginApi);
        var response = await _httpClient.PostAsync(buld.ToString(), 
            new StringContent(JsonSerializer.Serialize(new LoginModel
                {
                    Username = model.Username,
                    Password = model.Password
                }, JsonExtension.GetOptions()),
                Encoding.UTF8, ContentTypes.ApplicationJson));

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("User/Password incorrect!");
        }
        var loginResponse = await response.Content
            .ReadFromJsonAsync<AuthenticationViewModel>(JsonExtension.GetOptions());
        return loginResponse;
    }
    public async Task<string> Register(RegisterUser model)
    {
        var build = new StringBuilder();
        build.Append(_apiSetting.BaseUrl).Append(ApiPath.RegisterUserApi);
        var response = await _httpClient.PostAsync(build.ToString(), 
            new StringContent(JsonSerializer.Serialize(new RegisterUser
                {
                    Username = model.Username,
                    Password = model.Password,
                    Email = model.Email,
                    FullName = model.FullName,
                    IdNumber = model.IdNumber,
                    PhoneNumber = model.PhoneNumber
                    
                }, JsonExtension.GetOptions()),
                Encoding.UTF8, ContentTypes.ApplicationJson));

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }

        return "Successfully Registered";
    }

    public async Task<AuthenticationViewModel?> RefreshToken(RefreshTokenQuery query)
    {
  
        var response = await _httpClient.PostAsync(_apiSetting.BaseUrl+ApiPath.RefreshTokenApi, 
            new StringContent(JsonSerializer.Serialize( new RefreshTokenQuery
                    {
                        RefreshToken  = query.RefreshToken,
                        Username = query.Username
                    }
                    , JsonExtension.GetOptions()),
                Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            _navigationManager.NavigateTo("/lab-test", true);
        }
        var result = await response.Content
            .ReadFromJsonAsync<AuthenticationViewModel>(JsonExtension.GetOptions());
        var store = new SessionStore
        {
            Authentication = result
        };
        await   _sessionStoreService.Set(store);
        await _authStateProvider.MarkUserAsAuthenticated(result?.Username);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, result.Token);
        return result;
    }


    public async void Logout()
    {
        await  _sessionStoreService.RemoveAsync();
        ((BearerAuthStateProvider)_authStateProvider).MarkUserLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}