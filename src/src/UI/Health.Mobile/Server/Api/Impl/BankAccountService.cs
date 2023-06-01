using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Extensions;
using Health.Mobile.Server.Models.BankAccounts;


namespace Health.Mobile.Server.Api.Impl;

public class BankAccountService: IBankAccountService
{
    private readonly ApiSetting _apiSetting;
    private readonly HttpClient _httpClient;
    private readonly SessionStoreService _sessionStore;

    public BankAccountService(SessionStoreService sessionStore, HttpClient httpClient, ApiSetting apiSetting)
    {
        _sessionStore = sessionStore;
        _httpClient = httpClient;
        _apiSetting = apiSetting;
    }

    public async Task<List<BankAccountDetail>> GetBankAccounts()
    {
        var baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        build.Append(baseAddress)
            .Append(ApiPath.BankAccountApi);
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
            .ReadFromJsonAsync<IEnumerable<BankAccountDetail>>(JsonExtension.GetOptions());

        if (response == null)
        {
            throw new Exception("No data found");
        }

        return response.ToList();

    }

    public async Task<string> CreateBankAccount(BankAccount model)
    {
        string baseAddress = _apiSetting.BaseUrl;
        StringBuilder build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.BankAccountApi);
        var session = await  _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PostAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new BankAccount
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
        string baseAddress = _apiSetting.BaseUrl;
        StringBuilder build = new StringBuilder();
        build.Append(baseAddress).Append(ApiPath.BankAccountApi);
        var session = await _sessionStore.Get();
        if(session==null)
        {
            throw new Exception("Login required");
        }
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, session?.Authentication?.Token);
        var response = await _httpClient.PutAsync(build.ToString(), new StringContent(JsonSerializer.Serialize(new BankAccountDetail
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