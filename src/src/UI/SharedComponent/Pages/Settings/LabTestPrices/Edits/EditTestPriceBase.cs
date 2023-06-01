using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.Server.Models.TestPrices;

namespace SharedComponent.Pages.Settings.LabTestPrices.Edits;

public class EditTestPriceBase : ComponentBase
{
    [Parameter] public TestPriceDetail Model { get; set; } = new();
    [Inject] private ITestPriceService TestPriceService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    protected IList<LabTestDetail> LabTestDetails { get; set; } = new List<LabTestDetail>();
    protected bool IsSpinner = true;
    protected override async Task OnParametersSetAsync()
    {
        LabTestDetails = await LabTestService.GetLabTest();
        IsSpinner=false;
    }

    protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
            var result = await TestPriceService.UpdateTestPrice(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            IsSpinner=false;
            DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        Model.Price = 0;
        Model.LabTestId = null;
    }
}