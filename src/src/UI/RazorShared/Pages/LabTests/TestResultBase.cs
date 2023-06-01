using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Enums;
using RazorShared.Server.Models.LabTestResults;
using RazorShared.Server.Models.SelectedTestDetails;
using RazorShared.Server.Models.SelectedTestStatuses;

namespace RazorShared.Pages.LabTests;

public class TestResultBase : ComponentBase
{
    [Inject] private ITestResultService ResultService { get; set; } = null!;
    [Inject] private  ISelectedTestStatusService SelectedTestStatus { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService{get; set; }
    [Parameter] public SelectedTestDetail SelectedTestDetail { get; set; } = null!;
    protected TestResultDetail? Model { get; set; } = new();
    protected bool IsFinish = false;
    protected bool IsEdit = false;
    protected bool HasPermission;
     protected override async Task OnInitializedAsync()
     {
         var session=   await SessionStoreService.Get();
         if (session.Authentication?.Roles != null && session.Authentication != null && (session.Authentication.Roles.Contains("administrator")|| session.Authentication.Roles.Contains("supervisor")) )
         {
             HasPermission = true;
         }

         await GetData();
         if (Model.Id != null)
         {
             IsEdit = true;
             IsFinish = true;
         }
     }

     private async Task GetData()
     {
         Model=    await ResultService.Get(SelectedTestDetail.TransactionNumber);
     }
     protected void OnBack()
    {
        DialogService.Close();
    }

    protected async Task OnSave()
    {
        if (Model is {Id: null} && !IsEdit)
        {
            try
            {
                var testResult = new RazorShared.Server.Models.LabTestResults.TestResult
                {
                    Description = Model.Description,
                    TransactionNumber = SelectedTestDetail.TransactionNumber,
                    PatientId = SelectedTestDetail.PatientId
                };
                var result = await ResultService.Create(testResult);
                IsFinish = true;
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                StateHasChanged();
            }
            catch (Exception e)
            {
                NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }

        }
        else
        {
            try
            {
                var testResult = new TestResultDetail
                {
                    Description = Model?.Description,
                    Id = Model!.Id,
                };
                var result = await ResultService.Update(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                StateHasChanged();
            }
            catch (Exception e)
            {
                NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }
        }
    }

    protected async Task OnFinish()
    {
        try
        {
            if (HasPermission )
            {
                if (Model is {IsCompleted: true})
                {
                    var testResult = new SelectedTestStatus
                    {
                        TransactionNumber = SelectedTestDetail.TransactionNumber,
                        TestStatus = TestStatus.Completed
                    };
                    var result=     await SelectedTestStatus.Update(testResult);
                    NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                    DialogService.Close();
                    
                }
                else
                {
                    if (Model != null)
                    {
                        var approval = new LabTestResultApproval
                        {
                            IsCompleted = Model.IsCompleted,
                            Id = Model.Id,
                            Reason = Model.Reason,
                        };
                        var result=    await ResultService.Approval(approval);
                        NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                        DialogService.Close();
                    }
                }
            }
            else
            {
                var testResult = new TestResultDetail
                {
                    Description = Model?.Description,
                    Id = Model!.Id,
                    IsCompleted = true
                };
                var result = await ResultService.Update(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                DialogService.Close();
            }

        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}