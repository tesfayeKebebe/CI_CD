using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Categories;
using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.LabTests;
using RazorShared.Server.Models.TestPrices;

namespace RazorShared.Pages.Settings.LabTestPrices.Creates;
public  class CreateTestPriceBase : ComponentBase
{
    protected TestPrice Model { get; set; } = new ();
    [Inject] private ITestPriceService TestPriceService { get; set; } = null!;
     [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    protected IList<LabTestDetail> LabTestDetails { get; set; } = new List<LabTestDetail>();
     protected override async Task OnParametersSetAsync()
     {
         LabTestDetails = await LabTestService.GetLabTest();
     }
     protected async void OnSave()
    {
        try
        {
          var result= await TestPriceService.CreateTestPrice(Model);
          NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
          DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        Model = new ();
    }
}