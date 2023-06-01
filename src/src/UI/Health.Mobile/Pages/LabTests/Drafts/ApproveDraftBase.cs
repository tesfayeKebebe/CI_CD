
using Health.Mobile.Pages.ViewModel;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Enums;
using Health.Mobile.Server.Models.SelectedTestStatuses;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Health.Mobile.Pages.LabTests.Drafts;

public class ApproveDraftBase : ComponentBase
{
    [Inject] private  ISelectedTestStatusService SelectedTestStatus { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Parameter] public SelectedLabTestDetailViewModel SelectedTestDetail { get; set; } = null!;
    protected async Task OnApprove()
    {
        try
        {
            var testResult = new SelectedTestStatus
            {
                TransactionNumber = SelectedTestDetail.TransactionNumber,
                TestStatus = TestStatus.OnProgress
            };
            await SelectedTestStatus.Update(testResult);
            DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}