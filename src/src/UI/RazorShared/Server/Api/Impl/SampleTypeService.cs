using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Common;
using RazorShared.Server.Models.Sample_Type;
using System.Net.Http.Headers;
using System.Text;
using RazorShared.Server.Constants;
using RazorShared.Server.Extensions;
using RazorShared.Server.Models.Labs;
using System.Net.Http.Json;
using System.Text.Json;


namespace RazorShared.Server.Api.Impl
{
    public class SampleTypeService : ISampleTypeService
    {
        private readonly ApiSetting _apiSetting;
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public SampleTypeService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
        {
            _apiSetting = apiSetting;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<string> CreateSampleType(SampleType model)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.SampleTypeApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new SampleType
            {
                Name = model.Name,
                CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered";
        }

        public async Task<string> DeleteSampleType(string id)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.SampleTypeApi);
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

        public async Task<List<SampleTypeDetail>?> GetSampleType()
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.SampleTypeApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
            httpResponseMessage.EnsureSuccessStatusCode();
            var labResponse = await httpResponseMessage.Content
                .ReadFromJsonAsync<IEnumerable<SampleTypeDetail>>(JsonExtension.GetOptions());

            if (labResponse == null)
            {
                throw new Exception("No data found");
            }

            return labResponse.ToList();
        }

        public async Task<string> UpdateSampleType(SampleTypeDetail model)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.SampleTypeApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new SampleTypeDetail
            {
                Name = model.Name,
                Id = model.Id,
                IsActive = model.IsActive,
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated";
        }
    }
}
