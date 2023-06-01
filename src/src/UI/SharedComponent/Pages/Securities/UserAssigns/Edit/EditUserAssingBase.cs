using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Security;
using SharedComponent.Server.Models.UserAssigns;
using SharedComponent.Server.Models.UserBranches;

namespace SharedComponent.Pages.Securities.UserAssigns.Edit;

public class EditUserAssignBase : ComponentBase
{
    [Parameter] public UserAssignDto Model { get; set; } = new ();
    [Inject] private IUserAssignService AssignService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private IUserBranchService BranchService  { get; set; } = null!;
    protected IList<UserBranchDetail>? BranchDetails { get; set; }
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected IList<ApplicationUser>? ApplicationUsers { get; set; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        var data = await AuthenticationService.GetAllUsers();
        ApplicationUsers = data.Where(x => x.IsEnabled).ToList();
      var branches  = await BranchService.GetUserBranch();
      if (branches != null)
      {
          BranchDetails = branches.Where(x => x.IsActive).ToList();
      }
        IsSpinner=false;
    }

    protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
            var result= await AssignService.UpdateUserAssign(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            IsSpinner=false;
            DialogService.Close();
        }
        catch (Exception e)
        {
            IsSpinner=false;
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        Model = new UserAssignDto();
    }
}