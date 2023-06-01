using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Constants;
using Web.UI.Server.Enums;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.LabTests;
using Web.UI.Server.Models.SelectedTestDetails;

namespace Web.UI.Server.Api.Impl;

public class SelectedTestDetailService:ISelectedTestDetailService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;
    public SelectedTestDetailService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
    {
        _apiSetting = apiSetting;
        _httpClient = httpClient;
        _sessionStore = sessionStore;
    }
    public async Task<IEnumerable<SelectedTestDetail>> GetSelectedTestDetails(TestStatus status)
    {
        StringBuilder buld = new StringBuilder();
        buld.Append(_apiSetting.BaseUrl).Append($"{ApiPath.SelectedTestDetailApi}/{status}");
        var httpResponseMessage = await _httpClient.GetAsync(buld.ToString());
        httpResponseMessage.EnsureSuccessStatusCode();

        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<IEnumerable<SelectedTestDetail>>(JsonExtension.GetOptions());

        if (response == null)
        {
            throw new Exception("No data found");
        }

        return response.ToList();
    }

    public async Task<string> Create(List<SelectedTestDetailModel> models)
    {
        
        var build = new StringBuilder();
        build.Append(_apiSetting.BaseUrl).Append(ApiPath.SelectedTestDetailApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(models, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }
        return "Successfully Registered"; ;
    }
}