using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Common;
using Web.UI.Server.Constants;
using Web.UI.Server.Extensions;
using Web.UI.Server.Models.Labs;
using System.Net.Http.Json;
using Web.UI.Server.Models.Categories;

namespace Web.UI.Server.Api.Impl
{
    public class LabCategoryService : ILabCategoryService
    {
        private readonly ApiSetting _apiSetting;
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
        public LabCategoryService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
        {
            _apiSetting = apiSetting;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
        }
        public async Task<IList<CategoryDetail?>> GetLabCategory()
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabCategoryApi);
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
                .ReadFromJsonAsync<IEnumerable<CategoryDetail>>(JsonExtension.GetOptions());

            if (labResponse == null)
            {
                throw new Exception("No lab category  found");
            }

            return labResponse.ToList();



        }

        public async Task<string> CreateLabCategory(Category model)
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabCategoryApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new Category
            {
                Name = model.Name, 
                LabId = model.LabId
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered"; ;



        }
        public async Task<string> UpdateLabCategory(CategoryDetail model)
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabCategoryApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new CategoryDetail
            {
                Name = model.Name,
                Id = model.Id, IsActive = model.IsActive
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated"; ;



        }
        public async Task<string> DeleteLabCategory(string id)
        {
            var build = new StringBuilder();
            build.Append(_apiSetting.BaseUrl).Append(ApiPath.LabCategoryApi);
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
