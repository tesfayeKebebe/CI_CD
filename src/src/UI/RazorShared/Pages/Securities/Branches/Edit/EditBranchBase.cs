using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.UserBranches;

namespace RazorShared.Pages.Securities.Branches.Edit;

public class EditBranchBase : ComponentBase
{
    [Parameter] public UserBranchDetail Model { get; set; } = new();
    [Inject] private IUserBranchService BranchService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;

    protected async void OnSave()
    {
        try
        {
            var result = await BranchService.UpdateUserBranch(Model);
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
        Model.Name = null;
    }
}