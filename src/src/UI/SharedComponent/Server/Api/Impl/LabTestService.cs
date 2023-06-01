using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.LabTests;
namespace SharedComponent.Server.Api.Impl
{
    public class LabTestService  : ILabTestService
    {

        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public LabTestService( HttpClient httpClient, SessionStoreService sessionStore)
        {
           
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<IList<LabTestCategory>> GetLabCategoryTest()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(ApiPath.LabTestCategoryApi);
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
                var httpResponseMessage = await _httpClient.GetAsync($"{ApiPath.LabTestApi}/LabTestDropDown" );
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(ApiPath.LabTestApi);
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(ApiPath.LabTestApi, new StringContent(JsonSerializer.Serialize(new LabTest
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(ApiPath.LabTestApi, new StringContent(JsonSerializer.Serialize(new LabTestDetail
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.DeleteAsync($"{ApiPath.LabTestApi}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Deleted");
            }
            return "Successfully Deleted"; ;



        }

        public async Task<LabTestDetailById> GetLabTestById(string id)
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync($"{ApiPath.LabTestApi}/{id}");
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
