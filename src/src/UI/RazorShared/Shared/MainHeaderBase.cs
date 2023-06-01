using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Securities;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.StateManagement;

namespace RazorShared.Shared
{
    public class MainHeaderBase : ComponentBase , IDisposable
    { 
         protected int HeaderTabIndex { get; set; }
         [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
         [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private  NavigationStateService NavigationStateService { get; set; } = null!;
        [Inject] private DialogService DialogService { get; set; } = null!;
        [Inject] private BearerAuthStateProvider BearerAuthStateProvider { get; set; } = null!;
        [Inject] private  SessionStoreService SessionStoreService { get; set; } = null!;
        [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
        protected bool IsAuthenticated { get; set; }
        private PeriodicTimer _timer= new (TimeSpan.FromMinutes(1)) ;
        protected string? UserName { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            NavigationStateService.OnHeaderTabIndexChanged += OnHeaderTabIndexChanged;
            HeaderTabIndex = NavigationStateService.HeaderTabIndex;
            Show(NavigationStateService.HeaderTabIndex);
            LabTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
            var user =await BearerAuthStateProvider.GetAuthenticationStateAsync();
            if (user.User.Identity is {IsAuthenticated: true})
            {
                var session = await SessionStoreService.Get();
                UserName = session?.Authentication?.Username;
                IsAuthenticated = true;
            }
            else
            {
                while ( await _timer.WaitForNextTickAsync())
                {
                    var result=      DialogService.OpenAsync<Login>("",
                            new Dictionary<string, object>(),
                            new DialogOptions { Width = "300px", Height = "100%", Style = "position: absolute; Top:40px; right:2px;" });
                        StateHasChanged();
                }
            }
            await InvokeAsync(StateHasChanged);
        }
        

        private void OnLabTestChanged()
        {
            StateHasChanged();
        }
        private void OnHeaderTabIndexChanged()
        {
            HeaderTabIndex = NavigationStateService.HeaderTabIndex;
        }
        protected void Show(int index)
        {
            NavigationStateService.SetHeaderTabIndex(index);

            switch (index)
            {
                case 1:
                    NavigationManager.NavigateTo("/lab-test");
                    break;
                case 2:
                    NavigationManager.NavigateTo("/lab-test");
                    break;
                case 3:
                    NavigationManager.NavigateTo("/draft-list");
                    break;
                case 4:
                    NavigationManager.NavigateTo("/on-progress-lab-test");
                    break;
                case 5:
                    NavigationManager.NavigateTo("/completed-lab-test");
                    break;
                case 6:
                    NavigationManager.NavigateTo("/setting-tabs");
                    break;
                case 7:
                    NavigationManager.NavigateTo("/account-setting");
                    break;
                case 8:
                    NavigationManager.NavigateTo("/location");
                    break;
            }
            StateHasChanged();
        }
        protected async Task Login()
        {
       var result=     await DialogService.OpenAsync<Login>("",
            new Dictionary<string, object>(),
            new DialogOptions { Width = "300px", Height = "100%", Style = "position: absolute; Top:40px; right:2px;" });
            StateHasChanged();
        }
        

        protected async Task OnAccount()
        {
            var result=     await DialogService.OpenAsync<Account>("",
                new Dictionary<string, object>(),
                new DialogOptions { Width = "300px", Height = "100%", Style = "position: absolute; Top:40px; right:2px;" });
            StateHasChanged();
            // AuthenticationService.Logout();
            // IsAuthenticated = false;
            // NavigationManager.NavigateTo("/lab-test",true);
            await  InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            DialogService.Dispose();
            LabTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
            NavigationStateService.OnHeaderTabIndexChanged -= OnHeaderTabIndexChanged;
        }
    }
}