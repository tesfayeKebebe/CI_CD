using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models.Security;

namespace RazorShared.Pages.Securities;

public class ChangePasswordBase : ComponentBase
{
    protected ChangePasswordModel Model { get; set; } = new();
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        var session = await SessionStoreService.Get();
        Model.UserName = session?.Authentication?.Username;
    }

    protected async Task OnSubmit()
    {
        try
        {
            var result= await AuthenticationService.ChangePassword(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 5000);
            StateHasChanged();
            DialogService.Close();
            await  SessionStoreService.RemoveAsync();
            NavigationManager.NavigateTo("/login", true);
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
  
       
    }
}