using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Security;
using SharedComponent.Server.Models.SelectedTestStatuses;
using SharedComponent.Server.Models.UserAssigns;

namespace SharedComponent.Pages.LabTests;

public class EditAssignUserBase: ComponentBase
{
    [Parameter] public UpdateAssignedUser Model { get; set; } = new ();
    [Inject] private IUserAssignService UserAssignService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected IList<UserAssignDetail>? UserAssignDetails { get; set; }
    [Inject] private  ISelectedTestStatusService SelectedTestStatusService { get; set; }= null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        var data = await UserAssignService.GetUserAssign();
        UserAssignDetails = data?.Where(x => x.IsActive).ToList();
    }

    protected async void OnSave()
    {
        try
        {
            if (Model.AssignedUser == null)
            {
                return;
            }

            var result= await SelectedTestStatusService.AssignUser(Model);
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
        Model.AssignedUser = null;
    }
}