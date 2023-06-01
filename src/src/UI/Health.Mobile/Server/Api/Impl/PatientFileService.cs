using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models.PatientFiles;
namespace Health.Mobile.Server.Api.Impl;

public class PatientFileService:IPatientFileService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;
    public PatientFileService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
    {
        _apiSetting = apiSetting;
        _httpClient = httpClient;
        _sessionStore = sessionStore;
    }

    public async Task<List<PatientFileDetail>> GetPatientFiles()
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();

        build.Append(baseAddress)
            .Append(ApiPath.PatientFileApi);
        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<IEnumerable<PatientFileDetail>>(JsonExtension.GetOptions());
        if (response == null)
        {
            throw new Exception("No data found");
        }
        return response.ToList();
    }

    public async Task<IList<PatientFileDetail>> GetPatientFilesByPatientId()
    {
        var baseAddress = _apiSetting.BaseUrl;

        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        var build = new StringBuilder();
        build.Append(baseAddress)
            .Append(ApiPath.PatientFileApi).Append("/").Append(Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)));
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<IEnumerable<PatientFileDetail>>(JsonExtension.GetOptions());
        if (response == null)
        {
            throw new Exception("No data found");
        }
        return response.ToList();
    }

    public async Task<string> Create(List<PatientFileModel> models)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.PatientFileApi);
        var session = await  _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }

        foreach (var model in models)
        {
            model.CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!));
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PostAsync(build.ToString(),new StringContent(JsonSerializer.Serialize(models, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }
        return "Successfully Registered";
    }

    public async Task<string> Update(PatientFileDetail model)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.PatientFileApi);
        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new PatientFileDetail
        {
            Id = model.Id,
            ContentType = model.ContentType,
            FileName = model.FileName,
            PatientId = model.PatientId,
            StoredFileName = model.StoredFileName,
            LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
        }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
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
        build.Append(baseAddress).Append(ApiPath.PatientFileApi);
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

    public async Task<string> DeleteByPatientId(string patientId)
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.PatientFileApi);
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.DeleteAsync(build.Append('/').Append("patient-file/").Append(patientId).ToString());
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Deleted");
        }
        return "Successfully Deleted"; 
    }
}