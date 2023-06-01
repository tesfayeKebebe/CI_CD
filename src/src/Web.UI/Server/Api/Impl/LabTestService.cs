using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Constants;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.Labs;
using Web.UI.Server.Models.LabTests;

namespace Web.UI.Server.Api.Impl
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
            StringBuilder buld = new StringBuilder();
            buld.Append(_apiSetting.BaseUrl).Append(ApiPath.LabTestCategoryApi);
            var httpResponseMessage = await _httpClient.GetAsync(buld.ToString());
            httpResponseMessage.EnsureSuccessStatusCode();

            var labResponse = await httpResponseMessage.Content
                .ReadFromJsonAsync<IEnumerable<LabTestCategory>>(JsonExtension.GetOptions());

            if (labResponse == null)
            {
                throw new Exception("No data found");
            }

            return labResponse.ToList();



        }
        public async Task<List<LabTestDetail>> GetLabTest()
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabTestApi);
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
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabTestApi);
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
                TubeTypeId = model.TubeTypeId
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered"; ;



        }
        public async Task<string> UpdateLabTest(LabTestDetail model)
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabTestApi);
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
                TubeTypeId = model.TubeTypeId
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated"; ;



        }
        public async Task<string> DeleteLabTest(string id)
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabTestApi);
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

    }
}
