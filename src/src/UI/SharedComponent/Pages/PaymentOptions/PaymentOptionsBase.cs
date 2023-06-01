using System.Text;
using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Models;
using SharedComponent.Server.Models.BankAccounts;
using SharedComponent.Server.Models.Organizations;
using SharedComponent.Server.Models.SelectedTestDetails;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.PaymentOptions;

public class PaymentOptionsBase : ComponentBase, IDisposable
{
    [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IPaymentService PaymentService { get; set; } = null!;
    [Inject] private SessionStoreService SessionStoreService { get; set; }= null!;
    [Inject] private IJSRuntime JsRuntime { get; set; }= null!;
    protected List<BankAccountDetail> AccountDetails { get; set; } = new();
    [Inject] private IBankAccountService BankAccountService  { get; set; } = null!;
    private WindowNavigatorGeolocation _geolocation= null!;
    private IJSObjectReference _paymentModule;
    private GeolocationPosition _geolocationPosition= null!;
    protected long Copied { get; set; }
    protected double TotalPrice { get; set; }
    private SessionStore _sessionStore = new();
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnParametersSetAsync()
    {
        Interceptor.RegisterEvent();
        _sessionStore=   await SessionStoreService.Get();
        TotalPrice = LabTestCategoryStateServiceService.TotalPrice;
        AccountDetails = await GetBankInformation();
        IsSpinner=false;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            var window = await JsRuntime.Window();
            var navigator = await window.Navigator();
            _geolocation = navigator.Geolocation;
            _geolocationPosition = (await _geolocation.GetCurrentPosition(
                  new PositionOptions()
                  {
                      EnableHighAccuracy = true,
                      MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                      TimeoutTimeSpan = TimeSpan.FromMinutes(5)
                  })).Location;
            _paymentModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js");
        }

    }
    private async Task<List<BankAccountDetail>> GetBankInformation()
    {
        return await BankAccountService.GetBankAccounts();
    }
    private void OnLabTestChanged()
    {
          
    }

    protected async Task OnCopy(BankAccountDetail account)
    {
        await _paymentModule.InvokeVoidAsync("copy",account.Account);
        Copied = account.Account;
        StateHasChanged();
    }
    //private async ValueTask<string?> GetLocation()
    //{
    //    var location = await _paymentModule.InvokeAsync<string?>("getLocation");
    //    return location;
    //}
    protected async Task OnSubmit(int paymentId)
    {
        try
        {
            //var location = await GetLocation();
            //if(location!=null)
            //{

            //}
            IsSpinner=true;
            if (_geolocationPosition.Coords!=null)
            {
                var transactionNumber = DateTime.Now.Ticks.ToString();
                var models = new List<SelectedTestDetailModel>();
                foreach (var (_, category) in LabTestCategoryStateServiceService.ViewModels)
                {
                    foreach (var (__, test) in category.LabTests)
                    {
                        var model = new SelectedTestDetailModel
                        {
                            Price = test.Price,
                            LabTestId = test.Id
                            ,TransactionNumber =transactionNumber,
                            CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(_sessionStore?.Authentication?.UserId!))
                        };
                        models.Add(model);
                        LabTestCategoryStateServiceService.RemoveLabTest(test);
                    }
                }
                switch (paymentId)
                {
                    case 1:
                    {
                        var payment=   await PaymentService.PaymentByTeleBirr(models.Sum(x=>x.Price),_sessionStore?.Authentication?.Username);
                        if (payment != null)
                        {
                            var result= await SelectedTestDetailService.Create(models,TestStatus.OnProgress,_geolocationPosition.Coords.Latitude , _geolocationPosition.Coords.Longitude);
                            if (result != "")
                            {
                                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                            }
                            //_paymentModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
                            await JsRuntime.InvokeVoidAsync("load",payment);
                            //NavigationManager.NavigateTo(url, false);//opens the new page on same browser tab
                            // object?[]? values = { payment, "_blank" };
                            // CancellationToken token = new CancellationToken(false);
                            // await JSRuntime.InvokeAsync<object>("open", token, values);
                            // NavigationManager.NavigateTo(payment, true);
                            // var result= await SelectedTestDetailService.Create(models,TestStatus.OnProgress,GeolocationPosition.Coords.Latitude , GeolocationPosition.Coords.Longitude);
                            // NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);  
                            // NavigationManager.NavigateTo("/lab-test");
                        }

                    }
                        break;
                    case 2:
                    {
                        var result= await SelectedTestDetailService.Create(models,TestStatus.Draft,_geolocationPosition.Coords.Latitude , _geolocationPosition.Coords.Longitude);
                        if (result != "")
                        {
                            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);   
                            NavigationManager.NavigateTo("/lab-test");
                        }
                    }
                        break;
                }  
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, "", "Please enable your location!", 6000);  

            }
            IsSpinner=false;

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", ex.Message, 6000);
        }

    }

    protected void OnBack()
    {
        NavigationManager.NavigateTo("/selected-lab-test");
    }

    public void Dispose()
    {
        LabTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
       // _paymentModule.DisposeAsync();
    }
}