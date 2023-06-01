using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Security;

namespace Web.UI.Pages.Authentication;

public class RegisterBase : ComponentBase, IDisposable
{
    protected RegisterUser model { get; set; } = new();
    [Inject] private  IAuthenticationService _authenticationService { get; set; }
    [Inject] private  NotificationService _notificationService { get; set; }
    [Inject] private  DialogService _dialogService { get; set; }
    protected async Task OnSubmit()
    {
        try
        {
            var result= await _authenticationService.Register(model);
            _notificationService.Notify(NotificationSeverity.Success, "", result, 5000);
            await _dialogService.OpenAsync<Login>("",
                new Dictionary<string, object>(),
                new DialogOptions { Width = "300px", Height = "570px", Style = "position: absolute; Top:40px; right:2px;" });
            StateHasChanged();
        }
        catch (Exception e)
        {
          _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
  
       
    }

    public void Dispose()
    {
     _dialogService.Dispose();
    }
}