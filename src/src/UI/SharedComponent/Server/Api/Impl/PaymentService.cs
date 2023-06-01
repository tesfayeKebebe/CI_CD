
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SharedComponent.Server.Api.Impl;
public class PaymentService: IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;

    public PaymentService(HttpClient httpClient, SessionStoreService sessionStore)
    {
        _httpClient = httpClient;
        _sessionStore = sessionStore;
       
    }

    public async Task<string?> PaymentByTeleBirr(double amount, string userName)
    {
        try
        {
            const string url = "http://196.188.120.3:11443/service-openup/toTradeWebPay";
            var httpResponseMessage = await _httpClient.PostAsync(ApiPath.PaymentProviderApi, new StringContent(JsonSerializer.Serialize(new TelebirrPaymentRequest
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
        catch (Exception)
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