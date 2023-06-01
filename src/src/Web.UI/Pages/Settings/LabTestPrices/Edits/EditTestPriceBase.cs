using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.LabTests;
using Web.UI.Server.Models.TestPrices;

namespace Web.UI.Pages.Settings.LabTestPrices.Edits;

public class EditTestPriceBase : ComponentBase
{
    [Parameter] public TestPriceDetail model { get; set; } = new();
    [Inject] private ITestPriceService _testPriceService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    [Inject] private ILabTestService _labTestService { get; set; }
    protected IList<LabTestDetail?> labTestDetails { get; set; } = new List<LabTestDetail>();
    protected override async Task OnParametersSetAsync()
    {
        labTestDetails = await _labTestService.GetLabTest();
    }

    protected async void OnSave()
    {
        try
        {
            var result = await _testPriceService.UpdateTestPrice(model);
            _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            _dialogService.Close();
        }
        catch (Exception e)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        model.Price = 0;
        model.LabTestId = null;
    }
}