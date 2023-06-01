using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Constants;
using RazorShared.Server.Models;
namespace RazorShared.Server.Api.Impl;
public class PaymentService: IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;
    private readonly ApiSetting _apiSetting;
    public PaymentService(HttpClient httpClient, SessionStoreService sessionStore, ApiSetting apiSetting)
    {
        _httpClient = httpClient;
        _sessionStore = sessionStore;
        _apiSetting = apiSetting;
    }

    public async Task<string?> PaymentByTeleBirr(double amount)
    {
        try
        {
            const string url = "http://196.188.120.3:11443/service-openup/toTradeWebPay";

            var session = await _sessionStore.Get();
            if(session==null)
            {
                throw new Exception("Login required");
            }
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress)
                .Append(ApiPath.PaymentProviderApi).Append('?').Append("userName=").Append(session?.Authentication?.Username)
                .Append("&amount=").Append(amount);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
            var httpResponseMessage = await _httpClient.GetAsync(build.ToString());
            httpResponseMessage.EnsureSuccessStatusCode();
            var createPayIn = await httpResponseMessage.Content
                .ReadFromJsonAsync<CreatePayInRequest>();
            if (createPayIn == null)
            {
                throw new Exception("No data found");
            }
            var body = JsonConvert.SerializeObject(createPayIn);
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient
                .PostAsync(url, stringContent, CancellationToken.None)
                .ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<NotificationResponse>();
                return responseBody?.data.toPayUrl;
            }
            else
            {
                return "Fail to payed. Try again";
            }
        }
        catch (Exception )
        {
            throw new Exception("Fail to payed. Try again");
        }
      
        
        
    }
}
public class NotificationResponse
{
    public int code { get; set; }
    public string message { get; set; }
    public NotificationUrl data { get; set; }
    public long dateTime { get; set; }
}
public class NotificationUrl
{
    public string? toPayUrl { get; set; }
}