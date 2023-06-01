using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Enums;
using RazorShared.Server.Models.SelectedTestDetails;
using RazorShared.Server.Models.SelectedTestStatuses;

namespace RazorShared.Pages.LabTests.Drafts;

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