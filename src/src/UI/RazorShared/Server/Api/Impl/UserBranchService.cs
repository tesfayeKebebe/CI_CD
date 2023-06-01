using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Constants;
using RazorShared.Server.Extensions;
using RazorShared.Server.Models.UserBranches;

namespace RazorShared.Server.Api.Impl;

public class UserBranchService : IUserBranchService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;

    public UserBranchService(SessionStoreService sessionStore, HttpClient httpClient, ApiSetting apiSetting)
    {
        _sessionStore = sessionStore;
        _httpClient = httpClient;
        _apiSetting = apiSetting;
    }

    public async Task<string> CreateUserBranch(UserBranch model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.UserBranchApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(
            new UserBranch
            {
                Name = model.Name,          
                CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }

        return "Successfully Registered";
    }

    public async Task<string> DeleteUserBranch(string id)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.UserBranchApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.DeleteAsync(build.Append('/').Append(id).ToString());
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Deleted");
        }

        return "Successfully Deleted";
    }

    public async Task<List<UserBranchDetail>?> GetUserBranch()
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.UserBranchApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
        httpResponseMessage.EnsureSuccessStatusCode();
        var labResponse = await httpResponseMessage.Content
            .ReadFromJsonAsync<IEnumerable<UserBranchDetail>>(JsonExtension.GetOptions());

        if (labResponse == null)
        {
            throw new Exception("No data found");
        }

        return labResponse.ToList();
    }

    public async Task<string> UpdateUserBranch(UserBranchDetail model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.UserBranchApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(
            new UserBranchDetail
            {
                Id = model.Id,
                IsActive = model.IsActive, Name = model.Name,
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }

        return "Successfully Updated";
    }
}