using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Security;

namespace RazorShared.Pages.Securities;

public class EditUserBase: ComponentBase
{
    [Parameter]public UserDto? Model { get; set; } 
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected async Task OnSubmit()
    {
        try
        {
            var result= await AuthenticationService.UpdateUser(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 5000);
            StateHasChanged();
            DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
  
       
    }
    
    
}