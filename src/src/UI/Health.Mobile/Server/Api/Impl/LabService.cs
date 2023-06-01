using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models.Labs;
using System.Net.Http.Headers;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;

namespace Health.Mobile.Server.Api.Impl
{
    public class LabService : ILabService
    {
        private readonly ApiSetting _apiSetting;
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
        public LabService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
        {
            _apiSetting = apiSetting;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<List<LabDetail>?> GetLab()
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress)
         .Append(ApiPath.LabApi);
            var session = await _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
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
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabApi);
            var session = await  _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new Lab
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
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabApi);
            var session = await _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new LabDetail
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
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabApi);
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

    }
}
