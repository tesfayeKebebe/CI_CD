using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.Securities;

public class AccountBase : ComponentBase
{
    [Inject] private  SessionStoreService SessionStoreService { get; set; } = null!;
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private  NavigationStateService NavigationStateService { get; set; } = null!;
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
                    new DialogOptions { Width = "300px", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px;"});
                break;
            case 2:
              await  SessionStoreService.RemoveAsync();
              NavigationStateService.SetHeaderTabIndex(0);
                NavigationManager.NavigateTo("/home", true);
                break;
        }

    }
}