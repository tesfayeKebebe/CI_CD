
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Health.Mobile.Server.Api.Impl;
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

    public async Task<string?> PaymentByTeleBirr(double amount, string userName)
    {
        try
        {
            const string url = "http://196.188.120.3:11443/service-openup/toTradeWebPay";
            var baseAddress = _apiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress)
                .Append(ApiPath.PaymentProviderApi);
            var httpResponseMessage = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new TelebirrPaymentRequest
            {
                UserName = userName,
                Amount = amount
            }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
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
public class TelebirrPaymentRequest
{
    public  string UserName { get; set; }
    public double Amount { get; set; }
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