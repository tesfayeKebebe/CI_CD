using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.LabTests;
using RazorShared.Server.Models.TestPrices;

namespace RazorShared.Pages.Settings.LabTestPrices.Edits;

public class EditTestPriceBase : ComponentBase
{
    [Parameter] public TestPriceDetail Model { get; set; } = new();
    [Inject] private ITestPriceService TestPriceService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
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
            var result = await TestPriceService.UpdateTestPrice(Model);
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
        Model.Price = 0;
        Model.LabTestId = null;
    }
}