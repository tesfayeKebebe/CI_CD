using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models.Security;

namespace Health.Mobile.Server.Api.Impl;

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
        try
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LoginApi);
            var response = await _httpClient.PostAsync(build.ToString(), 
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
        catch (Exception)
        {
            throw new Exception("User/Password incorrect!");
        }
        
    }
    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress)
            .Append(ApiPath.AccountGetUsersApi);
        var session = await _sessionStoreService.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<IEnumerable<ApplicationUser>>(JsonExtension.GetOptions());

        if (response == null)
        {
            throw new Exception("No data found");
        }

        return response.ToList();
    }
    public async Task<string> Register(RegisterUser model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.RegisterUserApi);
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

    public async Task<string> RegisterByAdmin(RegisterUser model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.RegisterUserByAdminApi);
        var response = await _httpClient.PostAsync(build.ToString(), 
            new StringContent(JsonSerializer.Serialize(new RegisterUser
                {
                    Username = model.Username,
                    Password = model.Password,
                    Email = model.Email,
                    FullName = model.FullName,
                    IdNumber = model.IdNumber,
                    PhoneNumber = model.PhoneNumber,
                     RoleDescription = model.RoleDescription,
                     RoleName = model.RoleName
                    
                }, JsonExtension.GetOptions()),
                Encoding.UTF8, ContentTypes.ApplicationJson));

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }

        return "Successfully Registered";
    }
    public async Task<string>  UpdateUser(UserDto? model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.AccountUpdateUserApi);
        var session = await _sessionStoreService.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new  UserDto
            {
                Id = model!.Id,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                IsEnabled = model.IsEnabled,
                Email = model.Email
            }  
        , JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated";



    }
    public async Task<string> Delete(string id)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append($"{ApiPath.AccountDeleteUserApi}/{id}");
        var session = await _sessionStoreService.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.DeleteAsync(build.ToString());
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Deleted");
        }

        return "Successfully Deleted";
    }
    public async Task<string>  ChangePassword(ChangePasswordModel model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.ChangePasswordApi);
        var session = await _sessionStoreService.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new ChangePasswordModel
        {
            UserName = model.UserName,
            NewPassword = model.NewPassword,
            OldPassword = model.OldPassword
            
        }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Changed");
        }
        return "Successfully Changed";



    }

    public async Task<AuthenticationViewModel?> RefreshToken(RefreshTokenQuery query)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.RefreshTokenApi);
        var response = await _httpClient.PostAsync(build.ToString(), 
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
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, result!.Token);
        return result;
    }


    public async void Logout()
    {
        await  _sessionStoreService.RemoveAsync();
        ((BearerAuthStateProvider)_authStateProvider).MarkUserLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}