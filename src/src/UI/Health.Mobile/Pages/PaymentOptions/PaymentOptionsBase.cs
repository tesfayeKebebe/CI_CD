using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Enums;
using Health.Mobile.Server.Models.SelectedTestDetails;
using Health.Mobile.StateManagement;
using Health.Mobile.Server.Models.BankAccounts;

namespace Health.Mobile.Pages.PaymentOptions;

public class PaymentOptionsBase : ComponentBase, IDisposable
{
    [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IPaymentService PaymentService { get; set; } = null!;
    [Inject] private SessionStoreService SessionStoreService { get; set; } = null!;
    protected List<BankAccountDetail> AccountDetails { get; set; } = new();
    [Inject] private IBankAccountService BankAccountService { get; set; } = null!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
    private Lazy<IJSObjectReference> PaymentModule = new();
    protected long Copied { get; set; }
    protected double TotalPrice { get; set; }
    private SessionStore SessionStore = new();
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    [Inject] private LocationInformationService LocationInformationService { get; set; } = null!;
    protected override async Task OnParametersSetAsync()
    {
        Interceptor.RegisterEvent();
        SessionStore=   await SessionStoreService.Get();
       await LocationInformationService.CheckAndRequestLocationPermission();
        TotalPrice = LabTestCategoryStateServiceService.TotalPrice;
        AccountDetails = await GetBankInformation();

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
        PaymentModule = new(await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
        await PaymentModule.Value.InvokeVoidAsync("copy", account.Account);
        Copied = account.Account;
        StateHasChanged();
    }
    protected async Task OnSubmit(int paymentId)
    {
        try
        {
            var location = await LocationInformationService.CheckAndRequestLocationPermission();
            if (location != null)
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
                            CreatedBy = Encoding.UTF8.GetString(Convert.FromBase64String(SessionStore?.Authentication?.UserId!))
                        };
                        models.Add(model);
                        LabTestCategoryStateServiceService.RemoveLabTest(test);
                    }
                }
                switch (paymentId)
                {
                    case 1:
                    {
                        var payment=   await PaymentService.PaymentByTeleBirr(models.Sum(x=>x.Price),SessionStore?.Authentication?.Username);
                        if (payment != null)
                        {
                            var result= await SelectedTestDetailService.Create(models,TestStatus.OnProgress, location.Latitude , location.Longitude);
                            if (result != "")
                            {
                                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                            }
                            PaymentModule = new(await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
                            await PaymentModule.Value.InvokeVoidAsync("load",payment);
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
                        var result= await SelectedTestDetailService.Create(models,TestStatus.Draft, location.Latitude , location.Longitude);
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
    }
}