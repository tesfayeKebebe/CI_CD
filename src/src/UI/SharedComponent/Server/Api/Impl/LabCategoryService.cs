using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Common;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.Labs;
using System.Net.Http.Json;
using SharedComponent.Server.Models.Categories;
namespace SharedComponent.Server.Api.Impl
{
    public class LabCategoryService : ILabCategoryService
    {
       
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public LabCategoryService( HttpClient httpClient, SessionStoreService sessionStore)
        {
            _httpClient = httpClient;
            _sessionStore = sessionStore;
        }
        public async Task<List<CategoryDetail>?> GetLabCategory()
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(ApiPath.LabCategoryApi);
            httpResponseMessage.EnsureSuccessStatusCode();
            var labResponse = await httpResponseMessage.Content
                .ReadFromJsonAsync<IEnumerable<CategoryDetail>>(JsonExtension.GetOptions());
            if (labResponse == null)
            {
                throw new Exception("No lab category  found");
            }
            return labResponse.ToList();
        }

        public async Task<string> CreateLabCategory(Category model)
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(ApiPath.LabCategoryApi, new StringContent(JsonSerializer.Serialize(new Category
            {
                Name = model.Name, 
                LabId = model.LabId,
                CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered";
        }
        public async Task<string> UpdateLabCategory(CategoryDetail model)
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(ApiPath.LabCategoryApi, new StringContent(JsonSerializer.Serialize(new CategoryDetail
            {
                Name = model.Name,
                Id = model.Id, IsActive = model.IsActive,
                LabId = model.LabId,
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated";
        }
        public async Task<string> DeleteLabCategory(string id)
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.DeleteAsync( $"{ApiPath.LabCategoryApi}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Deleted");
            }
            return "Successfully Deleted"; ;



        }
    }
}
