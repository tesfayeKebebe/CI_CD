using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Security;

namespace SharedComponent.Pages.Securities;

public class RegisterBase : ComponentBase, IDisposable
{
    [Parameter]public RegisterUser Model { get; set; } = new();
    [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Inject]  private SessionStoreService  SessionStoreService { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool HasPermission = false;
    protected List<Role> _roles = new List<Role>();
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
     var session=   await SessionStoreService.Get();
     if (session.Authentication?.Roles != null && session.Authentication != null && (session.Authentication.Roles.Contains("administrator")|| session.Authentication.Roles.Contains("supervisor")) )
     {
         _roles = session.Authentication.Roles.Contains("supervisor") ? Roles.GetAllRoles().Where(x => x.RoleName != "administrator").ToList() : Roles.GetAllRoles();
         HasPermission = true;
     }
    }

   

    protected async Task OnSubmit()
    {
        try
        {
            IsSpinner=true;
            if (Model.IsAdmin)
            {
                Model.RoleDescription = _roles.FirstOrDefault(x => x.RoleName == Model.RoleName)?.RoleDescription;
                var result= await AuthenticationService.RegisterByAdmin(Model);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 5000);
            }
            else
            {
                var result= await AuthenticationService.Register(Model);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 5000);
            }
            IsSpinner=false;
            DialogService.Close();
            await DialogService.OpenAsync<Login>("",
                new Dictionary<string, object>(),
                new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true, Style = "position: absolute; Top:40px; right:2px;" });
            StateHasChanged();
        }
        catch (Exception e)
        {
          NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
  
       
    }

    public void Dispose()
    {
     DialogService.Dispose();
    }
}