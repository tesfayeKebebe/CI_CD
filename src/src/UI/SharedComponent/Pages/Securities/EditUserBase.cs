using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Security;

namespace SharedComponent.Pages.Securities;

public class EditUserBase: ComponentBase
{
    [Parameter]public UserDto? Model { get; set; } 
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected bool IsSpinner = false;
    protected async Task OnSubmit()
    {
        try
        {
            IsSpinner=true;
            var result= await AuthenticationService.UpdateUser(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 5000);
            IsSpinner=false;
            StateHasChanged();
            DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
  
       
    }
    
    
}