using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.UserBranches;

namespace SharedComponent.Pages.Securities.Branches.Create;
public  class CreateBranchBase : ComponentBase
{
    protected UserBranch Model { get; set; } = new ();
     [Inject] private IUserBranchService BranchService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected bool IsSpinner = false;
    protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
          var result= await BranchService.CreateUserBranch(Model);
          NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            IsSpinner=false;
          DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        Model = new UserBranch();
    }
}