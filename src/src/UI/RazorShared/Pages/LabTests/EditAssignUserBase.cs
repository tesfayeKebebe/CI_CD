using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Security;
using RazorShared.Server.Models.SelectedTestStatuses;
using RazorShared.Server.Models.UserAssigns;

namespace RazorShared.Pages.LabTests;

public class EditAssignUserBase: ComponentBase
{
    [Parameter] public UpdateAssignedUser Model { get; set; } = new ();
    [Inject] private IUserAssignService UserAssignService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected IList<UserAssignDetail>? UserAssignDetails { get; set; }
    [Inject] private  ISelectedTestStatusService SelectedTestStatusService { get; set; }
    protected override async Task OnInitializedAsync()
    {
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