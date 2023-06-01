using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Constants;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.LabTestResults;

namespace Web.UI.Server.Api.Impl;

public class TestResultService:ITestResultService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;
    public TestResultService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
    {
        _apiSetting = apiSetting;
        _httpClient = httpClient;
        _sessionStore = sessionStore;
    }
    public async Task<TestResultDetail> Get(string parentId)
    {
        var build = new StringBuilder();
        build.Append(_apiSetting.BaseUrl).Append($"{ApiPath.TestResultApi}/{parentId}");
        var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<TestResultDetail>(JsonExtension.GetOptions());
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception("No data found");
        }

        return response;
    }

    public async Task<string> Create(TestResult model)
    {
        var build = new StringBuilder();
        build.Append(_apiSetting.BaseUrl).Append(ApiPath.TestResultApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TestResult
        {
            Description = model.Description,
            ParentId = model.ParentId,
            PatientId = model.PatientId
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }
        return "Successfully Registered"; ;
    }

    public async Task<string> Update(TestResultDetail model)
    {
        var build = new StringBuilder();
        build.Append(_apiSetting.BaseUrl).Append(ApiPath.TestResultApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TestResultDetail
        {
        Description = model.Description,
        Id = model.Id
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated"; ;
    }
}