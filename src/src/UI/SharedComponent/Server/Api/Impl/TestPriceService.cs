using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Common;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.TestPrices;
using System.Net.Http.Json;


namespace SharedComponent.Server.Api.Impl
{
    public class TestPriceService : ITestPriceService
    {

        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public TestPriceService( HttpClient httpClient, SessionStoreService sessionStore)
        {
           
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<List<TestPriceDetail>> GetTestPrice()
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(ApiPath.LabTestPriceApi);
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
         
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(ApiPath.LabTestPriceApi, new StringContent(JsonSerializer.Serialize(new TestPrice
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(ApiPath.LabTestPriceApi, new StringContent(JsonSerializer.Serialize(new TestPriceDetail
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.DeleteAsync($"{ApiPath.LabTestPriceApi}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Deleted");
            }
            return "Successfully Deleted"; ;



        }
    }
}
