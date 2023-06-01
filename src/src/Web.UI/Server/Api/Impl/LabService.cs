using System.Net.Http.Json;
using System;
using System.Text;
using System.Text.Json;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Constants;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.Labs;
using System.Net.Http.Headers;
using System.Collections;
using Web.UI.Server.Common;
using Web.UI.Server.Common.Services;

namespace Web.UI.Server.Api.Impl
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
        public async Task<IList<LabDetail?>>  GetLab()
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabApi);
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
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabApi);
            var session = await  _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new Lab
            {
              Name  = model.Name
            }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered";



        }
        public async Task<string>  UpdateLab(LabDetail model)
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabApi);
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
                IsActive = model.IsActive
                 
            }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated";



        }
        public async Task<string> DeleteLab(string id)
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabApi);
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
