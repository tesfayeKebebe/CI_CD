using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.PatientFiles;
namespace SharedComponent.Server.Api.Impl;

public class PatientFileService:IPatientFileService
{
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;
    public PatientFileService( HttpClient httpClient, SessionStoreService sessionStore)
    {
       
        _httpClient = httpClient;
        _sessionStore = sessionStore;
    }

    public async Task<List<PatientFileDetail>> GetPatientFiles()
    {
        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync(ApiPath.PatientFileApi);
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

        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync($"{ApiPath.PatientFileApi}/{Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))}");
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
        var response = await _httpClient.PostAsync(ApiPath.PatientFileApi,new StringContent(JsonSerializer.Serialize(models, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }
        return "Successfully Registered";
    }

    public async Task<string> Update(PatientFileDetail model)
    {
        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(ApiPath.PatientFileApi, new StringContent(JsonSerializer.Serialize(new PatientFileDetail
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
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.DeleteAsync($"{ApiPath.PatientFileApi}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Deleted");
        }
        return "Successfully Deleted"; 

    }

    public async Task<string> DeleteByPatientId(string patientId)
    {
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.DeleteAsync($"{ApiPath.PatientFileApi}/patient-file/{patientId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Deleted");
        }
        return "Successfully Deleted"; 
    }
}