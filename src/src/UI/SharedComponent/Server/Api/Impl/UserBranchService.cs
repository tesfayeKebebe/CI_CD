using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.UserBranches;

namespace SharedComponent.Server.Api.Impl;

public class UserBranchService : IUserBranchService
{

    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;

    public UserBranchService(SessionStoreService sessionStore, HttpClient httpClient)
    {
        _sessionStore = sessionStore;
        _httpClient = httpClient;
       
    }

    public async Task<string> CreateUserBranch(UserBranch model)
    {
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PostAsync(ApiPath.UserBranchApi, new StringContent(JsonSerializer.Serialize(
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
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.DeleteAsync($"{ApiPath.UserBranchApi}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Deleted");
        }

        return "Successfully Deleted";
    }

    public async Task<List<UserBranchDetail>?> GetUserBranch()
    {
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync(ApiPath.UserBranchApi);
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
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(ApiPath.UserBranchApi, new StringContent(JsonSerializer.Serialize(
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