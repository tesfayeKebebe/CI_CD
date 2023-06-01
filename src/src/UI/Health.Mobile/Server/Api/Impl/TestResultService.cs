using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models.LabTestResults;

namespace Health.Mobile.Server.Api.Impl;

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
    public async Task<TestResultDetail> Get(string transactionNumber)
    {
        string BaseAddress = _apiSetting.BaseUrl;
        StringBuilder build = new StringBuilder();
        build.Append(BaseAddress).Append($"{ApiPath.TestResultApi}/{transactionNumber}");
        var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<TestResultDetail>(JsonExtension.GetOptions());
        if (response == null)
        {
            throw new Exception("No data found");
        }

        return response;
    }

    public async Task<string> Create(TestResult model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.TestResultApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        model.CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!));
        
        var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TestResult
        {
            Description = model.Description,
            TransactionNumber = model.TransactionNumber,
            PatientId = model.PatientId,
            CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)),
            //Attachments = model.Attachments,
            ContentType = model.ContentType,
            FileName = model.FileName,
            StoredFileName = model.StoredFileName
            
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }
        return "Successfully Registered"; ;
    }

    public async Task<string> Update(TestResultDetail model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.TestResultApi);
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
        Id = model.Id,
        IsCompleted = model.IsCompleted,
        // Attachments = model.Attachments,
        FileName = model.FileName,
        ContentType = model.ContentType,
        StoredFileName = model?.StoredFileName,
        LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated"; ;
    }
    public async Task<string> Approval(LabTestResultApproval model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.TestResultApi).Append("/Approval");
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new LabTestResultApproval
        {
            Reason = model.Reason,
            Id = model.Id,
            ApprovedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)),
            IsCompleted = model.IsCompleted
        }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated"; ;
    }
}