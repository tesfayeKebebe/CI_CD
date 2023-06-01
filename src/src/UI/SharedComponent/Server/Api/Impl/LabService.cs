using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.Labs;
using System.Net.Http.Headers;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;

namespace SharedComponent.Server.Api.Impl
{
    public class LabService : ILabService
    {
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
        public LabService( HttpClient httpClient, SessionStoreService sessionStore)
        {
           
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<List<LabDetail>?> GetLab()
        {
            var session = await _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(ApiPath.LabApi);
            httpResponseMessage.EnsureSuccessStatusCode();
            var labResponse = await httpResponseMessage.Content
                .ReadFromJsonAsync<IEnumerable<LabDetail>>(JsonExtension.GetOptions());

            if (labResponse == null)
            {
                throw new Exception("No lab found");
            }

            return labResponse.ToList();



        }
        
        public async Task<string>  CreateLab(Lab model)
        {
            var session = await  _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(ApiPath.LabApi, new StringContent(JsonSerializer.Serialize(new Lab
            {
              Name  = model.Name,
              CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered";



        }
        public async Task<string>  UpdateLab(LabDetail model)
        {
            var session = await _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(ApiPath.LabApi, new StringContent(JsonSerializer.Serialize(new LabDetail
            {
                Name  = model.Name,
                Id = model.Id,
                IsActive = model.IsActive,
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated";



        }
        public async Task<string> DeleteLab(string id)
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.DeleteAsync($"{ApiPath.LabApi}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Deleted");
            }
            return "Successfully Deleted"; 



        }

    }
}
