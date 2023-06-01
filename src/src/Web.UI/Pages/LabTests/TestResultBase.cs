using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Enums;
using Web.UI.Server.Models.LabTestResults;
using Web.UI.Server.Models.SelectedTestDetails;
using Web.UI.Server.Models.SelectedTestStatuses;

namespace Web.UI.Pages.LabTests;

public class TestResultBase : ComponentBase
{
    [Inject] private ITestResultService _resultService { get; set; }
    [Inject] private  ISelectedTestStatusService _selectedTestStatus { get; set; }
    [Inject] private  NotificationService _notificationService { get; set; }
    [Inject] private  DialogService _dialogService { get; set; }
    [Parameter] public SelectedTestDetail selectedTestDetail { get; set; }
    protected TestResultDetail? model { get; set; } = new();
    protected bool IsFinish = false;
    private bool IsEdit = false;
     protected override async Task OnInitializedAsync()
     {
         model=    await _resultService.Get(selectedTestDetail.ParentId);
         if (model.Id != null)
         {
             IsEdit = true;
             IsFinish = true;
         }
     }
     
     protected void OnBack()
    {
        _dialogService.Close();
    }

    protected async Task OnSave()
    {
        if (model is {Id: null} && !IsEdit)
        {
            try
            {
                var testResult = new Web.UI.Server.Models.LabTestResults.TestResult
                {
                    Description = model.Description,
                    ParentId = selectedTestDetail.ParentId,
                    PatientId = selectedTestDetail.PatientId
                };
                var result = await _resultService.Create(testResult);
                IsFinish = true;
                _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                StateHasChanged();
            }
            catch (Exception e)
            {
                _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }

        }
        else
        {
            try
            {
                var testResult = new Web.UI.Server.Models.LabTestResults.TestResultDetail
                {
                    Description = model?.Description,
                    Id = model!.Id
                };
                var result = await _resultService.Update(testResult);
                _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                StateHasChanged();
            }
            catch (Exception e)
            {
                _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }
        }
    }

    protected async Task OnFinish()
    {
        try
        {
            var testResult = new SelectedTestStatus
            {
            ParentId = selectedTestDetail.ParentId,
            TestStatus = TestStatus.Completed
            };
             await _selectedTestStatus.Update(testResult);
             _dialogService.Close();
        }
        catch (Exception e)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}