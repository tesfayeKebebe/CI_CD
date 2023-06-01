using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Common;
using RazorShared.Server.Constants;
using RazorShared.Server.Extensions;
using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.TestPrices;
using System.Net.Http.Json;


namespace RazorShared.Server.Api.Impl
{
    public class TestPriceService : ITestPriceService
    {
        private readonly ApiSetting _apiSetting;
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public TestPriceService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
        {
            _apiSetting = apiSetting;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<List<TestPriceDetail>> GetTestPrice()
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestPriceApi);
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
                .ReadFromJsonAsync<IEnumerable<TestPriceDetail>>(JsonExtension.GetOptions());

            if (labResponse == null)
            {
                throw new Exception("No lab found");
            }

            return labResponse.ToList();



        }

        public async Task<string> CreateTestPrice(TestPrice model)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestPriceApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TestPrice
            {
                LabTestId = model.LabTestId,
                Price = model.Price,
                CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson)); ;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered"; ;



        }
        public async Task<string> UpdateTestPrice(TestPriceDetail model)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestPriceApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TestPriceDetail
            {
                LabTestId = model.LabTestId,
                Price = model.Price ,
                IsActive = model.IsActive,
                Id = model.Id,
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated"; ;



        }
        public async Task<string> DeleteTestPrice(string id)
        {
            string baseAddress = _apiSetting.BaseUrl;
            StringBuilder build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.LabTestPriceApi);
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
