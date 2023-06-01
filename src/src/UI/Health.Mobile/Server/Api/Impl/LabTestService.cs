using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models.LabTests;
namespace Health.Mobile.Server.Api.Impl
{
    public class LabTestService  : ILabTestService
    {
        private readonly ApiSetting _apiSetting;
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public LabTestService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
        {
            _apiSetting = apiSetting;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<IList<LabTestCategory>> GetLabCategoryTest()
        {
            try
            {
                  string baseAddress = _apiSetting.BaseUrl;
                   StringBuilder build = new StringBuilder();
     
                    build.Append(baseAddress).Append(ApiPath.LabTestCategoryApi);
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(build.ToString());
                //var httpResponseMessage = await _httpClient.GetAsync(buld.ToString());
                httpResponseMessage.EnsureSuccessStatusCode();

                var labResponse = await httpResponseMessage.Content
                    .ReadFromJsonAsync<IEnumerable<LabTestCategory>>(JsonExtension.GetOptions());

                if (labResponse == null)
                {
                    throw new Exception("No data found");
                }

                return labResponse.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
         



        }
        public async Task<List<LabTestPriceDetail>> GetLabTestsDropDown()
        {
            try
            {
                var baseAddress = _apiSetting.BaseUrl;
                var build = new StringBuilder();
     
                build.Append(baseAddress).Append(ApiPath.LabTestApi).Append("/LabTestDropDown");
                var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
                //var httpResponseMessage = await _httpClient.GetAsync(buld.ToString());
                httpResponseMessage.EnsureSuccessStatusCode();
                var response = await httpResponseMessage.Content
                    .ReadFromJsonAsync<IEnumerable<LabTestPriceDetail>>(JsonExtension.GetOptions());
                if (response == null)
                {
                    throw new Exception("No data found");
                }

                return response.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
         



        }
        public async Task<List<LabTestDetail>> GetLabTest()
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestApi);
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
                .ReadFromJsonAsync<IEnumerable<LabTestDetail>>(JsonExtension.GetOptions());

            if (response == null)
            {
                throw new Exception("No data found");
            }

            return response.ToList();



        }

        public async Task<string> CreateLabTest(LabTest model)
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new LabTest
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Description = model.Description,
                IsFastingRequired = model.IsFastingRequired,
                SampleTypeId = model.SampleTypeId,
                TubeTypeId = model.TubeTypeId,
                CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered"; ;



        }
        public async Task<string> UpdateLabTest(LabTestDetail model)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new LabTestDetail
            {
                Name = model.Name,
                Id = model.Id,
                IsActive = model.IsActive,
                CategoryId= model.CategoryId ,
                Description = model.Description,
                IsFastingRequired = model.IsFastingRequired,
                SampleTypeId = model.SampleTypeId,
                TubeTypeId = model.TubeTypeId, 
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated"; ;



        }
        public async Task<string> DeleteLabTest(string id)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestApi);
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
            return "Successfully Deleted"; ;



        }

        public async Task<LabTestDetailById> GetLabTestById(string id)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestApi).Append('/').Append(id);
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
                .ReadFromJsonAsync<LabTestDetailById>(JsonExtension.GetOptions());

            if (response == null)
            {
                throw new Exception("No data found");
            }

            return response;
        }
    }
}
