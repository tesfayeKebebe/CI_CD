using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Extensions;
using SharedComponent.Server.Models.BankAccounts;


namespace SharedComponent.Server.Api.Impl;

public class BankAccountService: IBankAccountService
{

    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;

    public BankAccountService(SessionStoreService sessionStore, HttpClient httpClient)
    {
        _sessionStore = sessionStore;
        _httpClient = httpClient;
       
    }

    public async Task<List<BankAccountDetail>> GetBankAccounts()
    {
        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var httpResponseMessage = await _httpClient.GetAsync(ApiPath.BankAccountApi);
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content
            .ReadFromJsonAsync<IEnumerable<BankAccountDetail>>(JsonExtension.GetOptions());

        if (response == null)
        {
            throw new Exception("No data found");
        }

        return response.ToList();

    }

    public async Task<string> CreateBankAccount(BankAccount model)
    {
        var session = await  _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PostAsync(ApiPath.BankAccountApi, new StringContent(JsonSerializer.Serialize(new BankAccount
        {
            Name  = model.Name,
            CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)),
            Account = model.Account
        }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Registered");
        }
        return "Successfully Registered";
    }

    public async Task<string> UpdateBankAccount(BankAccountDetail model)
    {
        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(ApiPath.BankAccountApi, new StringContent(JsonSerializer.Serialize(new BankAccountDetail
        {
            Name  = model.Name,
            Id = model.Id,
            Account = model.Account,
            LastModifiedBy = Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
        }, JsonExtension.GetOptions()),Encoding.UTF8, ContentTypes.ApplicationJson ));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Not Updated");
        }
        return "Successfully Updated";
    }
}