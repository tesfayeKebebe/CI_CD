using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Constants;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.LabTests;
using Web.UI.Server.Models.SelectedTestStatuses;

namespace Web.UI.Server.Api.Impl;

public class SelectedTestStatusService : ISelectedTestStatusService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;
    public SelectedTestStatusService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
    {
        _apiSetting = apiSetting;
        _httpClient = httpClient;
        _sessionStore = sessionStore;
    }
    public async Task<string> Update(SelectedTestStatus model)
    {
        var build = new StringBuilder();
        build.Append(_apiSetting.BaseUrl).Append(ApiPath.SelectedTestStatusApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new SelectedTestStatus
        {
       ParentId = model.ParentId,
       TestStatus = model.TestStatus
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated"; ;
    }
}