using Microsoft.AspNetCore.Components;
using Radzen;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.Security;

namespace Health.Mobile.Pages.Securities;

public class ChangePasswordBase : ComponentBase
{
    protected ChangePasswordModel Model { get; set; } = new();
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
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