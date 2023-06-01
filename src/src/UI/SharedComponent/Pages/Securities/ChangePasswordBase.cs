using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Security;

namespace SharedComponent.Pages.Securities;

public class ChangePasswordBase : ComponentBase
{
    protected ChangePasswordModel Model { get; set; } = new();
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
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
            IsSpinner=true;
            var result= await AuthenticationService.ChangePassword(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 5000);
            IsSpinner=false;
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