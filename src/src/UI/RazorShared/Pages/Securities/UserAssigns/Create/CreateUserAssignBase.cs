using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Security;
using RazorShared.Server.Models.UserAssigns;
using RazorShared.Server.Models.UserBranches;

namespace RazorShared.Pages.Securities.UserAssigns.Create;
public  class CreateUserAssignBase : ComponentBase
{
    protected UserAssign Model { get; set; } = new ();
     [Inject] private IUserAssignService AssignService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private IUserBranchService BranchService  { get; set; } = null!;
    protected IList<UserBranchDetail>? BranchDetails { get; set; }
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected IList<ApplicationUser>? ApplicationUsers { get; set; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        var data = await AuthenticationService.GetAllUsers();
        ApplicationUsers = data.Where(x => x.IsEnabled).ToList();
        var branches  = await BranchService.GetUserBranch();
        if (branches != null)
        {
            BranchDetails = branches.Where(x => x.IsActive).ToList();
        }
    }

    protected async void OnSave()
    {
        try
        {
          var result= await AssignService.CreateUserAssign(Model);
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
        Model = new UserAssign();
    }
}