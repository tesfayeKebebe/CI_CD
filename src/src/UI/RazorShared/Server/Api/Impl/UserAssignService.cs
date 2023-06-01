using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Constants;
using RazorShared.Server.Extensions;
using RazorShared.Server.Models.UserAssigns;

namespace RazorShared.Server.Api.Impl;

public class UserAssignService :IUserAssignService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;
    public UserAssignService(SessionStoreService sessionStore, HttpClient httpClient, ApiSetting apiSetting)
    {
        _sessionStore = sessionStore;
        _httpClient = httpClient;
        _apiSetting = apiSetting;
    }
            public async Task<string> CreateUserAssign(UserAssign model)
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.UserAssignApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new UserAssign
            {
               UserId = model.UserId,
               UserBranchId = model.UserBranchId,
               CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Registered");
            }
            return "Successfully Registered";
        }

        public async Task<string> DeleteUserAssign(string id)
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.UserAssignApi);
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

        public async Task<List<UserAssignDetail>?> GetUserAssign()
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.UserAssignApi);
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
                .ReadFromJsonAsync<IEnumerable<UserAssignDetail>>(JsonExtension.GetOptions());

            if (labResponse == null)
            {
                throw new Exception("No data found");
            }

            return labResponse.ToList();
        }

        public async Task<string> UpdateUserAssign(UserAssignDto model)
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.UserAssignApi);
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new UserAssignDto
            {
               
                Id = model.Id,
                IsActive = model.IsActive
                , UserId = model.UserId,
                UserBranchId = model.UserBranchId,
                LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))

            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated";
        }
        public async Task<string> UpdateUserAssignByService(UserAssignByService model)
        {
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append(ApiPath.UserAssignApi).Append("/UpdateByService");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, model.Token);
            var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new UserAssignByService
            {
                UserId = model.UserId,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            }, JsonExtension.GetOptions()), Encoding.UTF8, ContentTypes.ApplicationJson));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Updated");
            }
            return "Successfully Updated";
        }
}