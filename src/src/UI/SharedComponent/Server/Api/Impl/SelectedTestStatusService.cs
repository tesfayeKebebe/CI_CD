using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.Server.Models.SelectedTestStatuses;

namespace SharedComponent.Server.Api.Impl;

public class SelectedTestStatusService : ISelectedTestStatusService
{

    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;

    public SelectedTestStatusService( HttpClient httpClient, SessionStoreService sessionStore)
    {
       
        _httpClient = httpClient;
        _sessionStore = sessionStore;

    }
    public async Task<string> Update(SelectedTestStatus model)
    {
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(ApiPath.SelectedTestStatusApi, new StringContent(JsonSerializer.Serialize(new SelectedTestStatus
        {
       TransactionNumber = model.TransactionNumber,
       TestStatus = model.TestStatus,
       LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated"; ;
    }
    public async Task<string> AssignUser(UpdateAssignedUser model)
    {
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync($"{ApiPath.SelectedTestStatusApi}/AssignUser", new StringContent(JsonSerializer.Serialize(new UpdateAssignedUser
        {
            TransactionNumber = model.TransactionNumber,
             AssignedUser = model.AssignedUser
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated"; ;
    }
    public async Task<string> UpdateIsSample(UpdateSelectedTestSample model)
    {
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync($"{ApiPath.SelectedTestStatusApi}/Sample", new StringContent(JsonSerializer.Serialize(new UpdateSelectedTestSample
        {
            TransactionNumber = model.TransactionNumber,
            IsSampleTaken = model.IsSampleTaken
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated"; ;
    }
}