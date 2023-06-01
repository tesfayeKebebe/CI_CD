using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Models.SelectedTestDetails;
using SharedComponent.Server.Models.SelectedTestStatuses;

namespace SharedComponent.Pages.LabTests.Drafts;

public class ApproveDraftBase : ComponentBase
{
    [Inject] private  ISelectedTestStatusService SelectedTestStatus { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Parameter] public SelectedLabTestDetailViewModel SelectedTestDetail { get; set; } = null!;
    protected bool IsSpinner = true;
    protected async Task OnApprove()
    {
        try
        {
            IsSpinner=true;
            var testResult = new SelectedTestStatus
            {
                TransactionNumber = SelectedTestDetail.TransactionNumber,
                TestStatus = TestStatus.OnProgress
            };
            await SelectedTestStatus.Update(testResult);
            IsSpinner=false;
            DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}