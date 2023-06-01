using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;

namespace RazorShared.Pages.Securities;

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
                    new DialogOptions {Width = "50%", Height = "570px"});
                break;
            case 2:
              await  SessionStoreService.RemoveAsync();
                NavigationManager.NavigateTo("/login", true);
                break;
        }

    }
}