using Microsoft.AspNetCore.Components;
using Radzen;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.Security;

namespace Health.Mobile.Pages.Securities
{
    public class LoginBase : ComponentBase, IDisposable
    {
        [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject]  private NotificationService NotificationService { get; set; } = null!;
        [Inject]  private  BearerAuthStateProvider BearerAuthStateProvider { get; set; } = null!;
        [Inject]  private SessionStoreService  SessionStoreService { get; set; } = null!;
        [Inject]   private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private DialogService DialogService { get; set; } = null!;
        [Parameter] public LoginModel Model { get; set; } = new LoginModel();
        [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
        protected async Task OnLogin()
        {
            try
            {
                Interceptor.RegisterEvent();
                var result = await AuthenticationService.Login(Model);
                if (result != null)
                {
                    var sessionStore = new SessionStore
                    {
                        Authentication = result
                    };
                    await   SessionStoreService.Set(sessionStore);
                    await BearerAuthStateProvider.MarkUserAsAuthenticated(result?.Username);
                }

                await InvokeAsync(StateHasChanged);
                //       _navigationManager
                // .NavigateTo( HttpUtility.UrlEncode(_navigationManager.ToBaseRelativePath(_navigationManager.Uri)));
                NavigationManager.NavigateTo("/lab-test", true);
                DialogService.Close();

            }
            catch (Exception e)
            {
                NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }
        }
        

        protected async Task OnRegister ()
        {
            DialogService.Close();
            await DialogService.OpenAsync<Register>("",
                new Dictionary<string, object>() ,
                new DialogOptions {Width = "40%", Height = "570px", Style = "position: absolute; Top:80px; " });
            DialogService.Close();
       
        }
       public void Dispose()
       {
           DialogService.Dispose();
        }
    }
}
