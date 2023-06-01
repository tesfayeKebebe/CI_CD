using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Common;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.Sample_Type;
using SharedComponent.Server.Models.Tube_Type;
using System.Net.Http.Json;


namespace SharedComponent.Server.Api.Impl
{
    public class TubeTypeService : ITubeTypeService
    {

        private readonly HttpClient _httpClient;
        private readonly SessionStoreService _sessionStore;
    
        public TubeTypeService( HttpClient httpClient, SessionStoreService sessionStore)
        {
           
            _httpClient = httpClient;
            _sessionStore = sessionStore;
    
        }

        public async Task<string> CreateTubeType(TubeType model)
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PostAsync(ApiPath.TubeTypeApi, new StringContent(JsonSerializer.Serialize(new TubeType
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.DeleteAsync($"{ApiPath.TubeTypeApi}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not Deleted");
            }
            return "Successfully Deleted";
        }

        public async Task<List<TubeTypeDetail>?> GetTubeType()
        {
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(ApiPath.TubeTypeApi);
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
            var session = await _sessionStore.Get();
            if (session == null)
            {
                throw new Exception("Login required");
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var response = await _httpClient.PutAsync(ApiPath.TubeTypeApi, new StringContent(JsonSerializer.Serialize(new TubeTypeDetail
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
