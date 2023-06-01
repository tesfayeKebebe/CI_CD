using Microsoft.AspNetCore.Components;
using Radzen;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common.Services;

namespace Health.Mobile.Pages.Securities;

public class AccountBase : ComponentBase
{
    [Inject] private  SessionStoreService SessionStoreService { get; set; } = null!;
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
    protected async Task GoTo(int id)
    {
        switch (id)
        {
            case 1:
                DialogService.Close();
                await DialogService.OpenAsync<ChangePassword>("",
                    new Dictionary<string, object>() ,
                    new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:80px; " });
                break;
            case 2:
              await  SessionStoreService.RemoveAsync();
                NavigationManager.NavigateTo("/login", true);
                break;
        }

    }
}