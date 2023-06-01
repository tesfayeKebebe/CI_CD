using Microsoft.AspNetCore.Components;
using Radzen;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Constants;
using Web.UI.Server.Models.Security;
using Web.UI.StateManagement;

namespace Web.UI.Pages.Authentication
{
    public class LoginBase : ComponentBase, IDisposable
    {
        [Inject] private IAuthenticationService _authenticationService { get; set; }
        [Inject]  private NotificationService _notificationService { get; set; }
        [Inject]  private  BearerAuthStateProvider _bearerAuthStateProvider { get; set; }
        [Inject]  private SessionStoreService  _sessionStoreService { get; set; }
        [Inject]   private NavigationManager _navigationManager { get; set; }
        [Inject] private DialogService _dialogService { get; set; } = null!;
        [Inject]  private HttpClient _httpClient { get; set; }
        protected LoginModel model { get; set; } = new LoginModel();

        protected async Task OnLogin()
        {
            try
            {
                var result = await _authenticationService.Login(model);
                if (result != null)
                {
                    var sessionStore = new SessionStore
                    {
                        Authentication = result
                    };
                    await   _sessionStoreService.Set(sessionStore);
                    await _bearerAuthStateProvider.MarkUserAsAuthenticated(result?.Username);
                }

                await InvokeAsync(StateHasChanged);
                //       _navigationManager
                // .NavigateTo( HttpUtility.UrlEncode(_navigationManager.ToBaseRelativePath(_navigationManager.Uri)));
                _navigationManager.NavigateTo("/lab-test", true);
                _dialogService.Close();

            }
            catch (Exception e)
            {
                _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }
        }
        

        protected void OnRegister ()
        {
            _navigationManager.NavigateTo("/register-user");
            _dialogService.Close();
        }
       public void Dispose()
       {
           _dialogService.Dispose();
        }
    }
}
