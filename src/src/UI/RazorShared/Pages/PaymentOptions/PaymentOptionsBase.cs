using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Enums;
using RazorShared.Server.Models.SelectedTestDetails;
using RazorShared.StateManagement;

namespace RazorShared.Pages.PaymentOptions;

public class PaymentOptionsBase : ComponentBase, IDisposable
{
    [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IPaymentService PaymentService { get; set; } = null!;
    [Inject] private IJSRuntime JSRuntime { get; set; }
    private WindowNavigatorGeolocation Geolocation;
    private GeolocationPosition GeolocationPosition;
    protected double TotalPrice { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        var window = await JSRuntime.Window();
        var navigator = await window.Navigator();
        Geolocation = navigator.Geolocation;
        TotalPrice = LabTestCategoryStateServiceService.TotalPrice;

    }

    private void OnLabTestChanged()
    {
          
    }
    protected async Task OnSubmit(int paymentId)
    {
        try
        {
            GeolocationPosition = (await Geolocation.GetCurrentPosition(
                new PositionOptions()
                {
                    EnableHighAccuracy = true,
                    MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                    TimeoutTimeSpan = TimeSpan.FromMinutes(5)
                })).Location;
            if (GeolocationPosition.Coords!=null)
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
                            ,TransactionNumber =transactionNumber
                        };
                        models.Add(model);
                        LabTestCategoryStateServiceService.RemoveLabTest(test);
                    }
                }
                switch (paymentId)
                {
                    case 1:
                    {
                        var payment=   await PaymentService.PaymentByTeleBirr(models.Sum(x=>x.Price));
                        if (payment != null)
                        {
                            NavigationManager.NavigateTo(payment, true);
                            var result= await SelectedTestDetailService.Create(models,TestStatus.OnProgress,GeolocationPosition.Coords.Latitude , GeolocationPosition.Coords.Longitude);
                            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);  
                            // NavigationManager.NavigateTo("/lab-test");
                        }

                    }
                        break;
                    case 2:
                    {
                        var result= await SelectedTestDetailService.Create(models,TestStatus.Draft,GeolocationPosition.Coords.Latitude , GeolocationPosition.Coords.Longitude);
                        NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);   
                        NavigationManager.NavigateTo("/lab-test");
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

        StateHasChanged();
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