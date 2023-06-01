using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models.Sample_Type;
using Health.Mobile.Server.Models.Tube_Type;
using System.Net.Http.Json;


namespace Health.Mobile.Server.Api.Impl
{
    public class TubeTypeService : ITubeTypeService
    {
        private readonly ApiSetting _apiSetting;
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public TubeTypeService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
        {
            _apiSetting = apiSetting;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }

        public async Task<string> CreateTubeType(TubeType model)
        {
            string BaseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(BaseAddress).Append(ApiPath.TubeTypeApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TubeType
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

        public async Task<string> DeleteTubeType(string id)
        {
            string BaseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(BaseAddress).Append(ApiPath.TubeTypeApi);
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

        public async Task<List<TubeTypeDetail>?> GetTubeType()
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.TubeTypeApi);
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
                .ReadFromJsonAsync<IEnumerable<TubeTypeDetail>>(JsonExtension.GetOptions());

            if (labResponse == null)
            {
                throw new Exception("No data found");
            }

            return labResponse.ToList();
        }

        public async Task<string> UpdateTubeType(TubeTypeDetail model)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.TubeTypeApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TubeTypeDetail
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
