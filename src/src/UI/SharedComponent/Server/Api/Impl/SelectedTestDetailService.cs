using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.Server.Models.SelectedTestDetails;

namespace SharedComponent.Server.Api.Impl;

public class SelectedTestDetailService:ISelectedTestDetailService
{

    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;

    public SelectedTestDetailService( HttpClient httpClient, SessionStoreService sessionStore)
    {
       
        _httpClient = httpClient;
        _sessionStore = sessionStore;

    }
    public async Task<List<SelectedTestDetail>> GetSelectedTestDetails(TestStatus status)
    {
        try
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            //_httpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync($"{ApiPath.SelectedTestDetailApi}/{status}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content
                .ReadFromJsonAsync<List<SelectedTestDetail>>(JsonExtension.GetOptions());

            if (response == null)
            {
                throw new Exception("No data found");
            }

            return response.ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
      
    }
    public async Task<List<SelectedTestDetail>> GetSelectedTestDetailsByUser()
    {
        try
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync($"{ApiPath.SelectedTestDetailApi}/SelectedData/{Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content
                .ReadFromJsonAsync<List<SelectedTestDetail>>(JsonExtension.GetOptions());

            if (response == null)
            {
                throw new Exception("No data found");
            }

            return response.ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
      
    }

    public async Task<string> DeleteSelectedDetailByTransaction(string transactionNumber)
    {
        var session = await _sessionStore.Get();
        if (session == null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.DeleteAsync($"{ApiPath.SelectedTestDetailApi}/{transactionNumber}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Deleted");
        }
        return "Successfully Deleted";
    }

    public async Task<List<SelectedTestDetail>> GetSelectedTestDetailsByDate( SelectedTestDetailQuery query)
    {
        try
        {
            var build = new StringBuilder();
            build.Append($"{ApiPath.SelectedTestDetailApi}?TestStatus={query.TestStatus}");
            if (query.From != null)
            {
                build.Append("&").Append("From").Append("=").Append(query.From);
            }
            if (query.From != null)
            {
                build.Append("&").Append("To").Append("=").Append(query.To);
            }
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content
                .ReadFromJsonAsync<List<SelectedTestDetail>>(JsonExtension.GetOptions());

            if (response == null)
            {
                throw new Exception("No data found");
            }

            return response.ToList();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
      
    }

    public async Task<string> Create(List<SelectedTestDetailModel> models, TestStatus status, double latitude, double longitude)
    {
        try
        {
            var build = new StringBuilder();
            build.Append(ApiPath.SelectedTestDetailApi)
                .Append('/').Append((int)status)
                .Append('/').Append(latitude)
                .Append('/').Append(longitude);
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
                throw new Exception("Failed to pay");
            }
            return "Successfully Payed"; ;
        }
        catch (Exception)
        {
            throw new Exception("Failed to pay");
        }
       
    }

    public async Task<IEnumerable<SelectedLabTestDetailByParentId>> GetLabTestById(string parentId)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"{ApiPath.SelectedTestDetailApi}/GetData/{parentId}");
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<IEnumerable<SelectedLabTestDetailByParentId>>(JsonExtension.GetOptions());
        if (response == null)
        {
            throw new Exception("No data found");
        }

        return response.ToList();
    }
}