using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using System.Net.Http.Headers;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.Organizations;

namespace Health.Mobile.Server.Api.Impl
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApiSetting _apiSetting;
        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
        public OrganizationService(ApiSetting apiSetting, HttpClient httpClient, SessionStoreService sessionStore)
        {
            _apiSetting = apiSetting;
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }
        public async Task<OrganizationDetail?> GetOrganization()
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress)
         .Append(ApiPath.OrganizationApi);
            var session = await _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content
                .ReadFromJsonAsync<OrganizationDetail>(JsonExtension.GetOptions());

            if (response == null)
            {
                throw new Exception("No data found");
            }

            return response;



        }
        
        public async Task<string>  CreateOrganization(OrganizationDetail? model)
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.OrganizationApi);
            var session = await  _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new Organization
            {
                Telephone = model.Telephone,
                Location = model.Location,
                About  = model.About,
                Email = model.Email,
                CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)),
            }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered";
        }
        public async Task<string>  UpdateOrganization(OrganizationDetail? model)
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.OrganizationApi);
            var session = await _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new OrganizationDetail
            {
                Telephone = model.Telephone,
                Location = model.Location,
                About  = model.About,
                Id = model.Id,
                Email = model.Email,
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated";



        }


    }
}
