using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Authentication;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common.Services;
using Web.UI.StateManagement;

namespace Web.UI.Shared
{
    public class MainHeaderBase : ComponentBase , IDisposable
    { 
         protected int HeaderTabIndex { get; set; }
         [Inject] private LabTestCategoryStateService _labTestCategoryStateServiceService { get; set; } = null!;
         [Inject]  private NavigationManager NavigationManager { get; set; }
         [Inject] private  NavigationStateService NavigationStateService { get; set; }
         [Inject] private DialogService _dialogService { get; set; } = null!;
        [Inject] private BearerAuthStateProvider _bearerAuthStateProvider { get; set; }
        [Inject] private  SessionStoreService _sessionStoreService { get; set; }
        [Inject] private  IAuthenticationService _authenticationService { get; set; }
        protected bool IsAuthenticated { get; set; }
        private readonly object _mutex = new();
        private PeriodicTimer? _timer;
        protected string? UserName { get; set; }
        protected override async Task OnInitializedAsync()
        {
           
        }

        protected override async Task OnParametersSetAsync()
        {
            NavigationStateService.OnHeaderTabIndexChanged += OnHeaderTabIndexChanged;
            HeaderTabIndex = NavigationStateService.HeaderTabIndex;
            Show(NavigationStateService.HeaderTabIndex);
            _labTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
            var user =await _bearerAuthStateProvider.GetAuthenticationStateAsync();
            if (user.User.Identity is {IsAuthenticated: true})
            {
                var session = await _sessionStoreService.Get();
                UserName = session?.Authentication?.Username;
                IsAuthenticated = true;
            }
            else
            {
                lock (_mutex)
                {
                    _timer ??= new PeriodicTimer(TimeSpan.FromSeconds(10));
                }
        
                while (_timer !=null && await _timer.WaitForNextTickAsync())
                {
                    lock (_mutex)
                    {
                        _timer?.Dispose();
                        _timer = null;
                        var result=      _dialogService.OpenAsync<Login>("",
                            new Dictionary<string, object>(),
                            new DialogOptions { Width = "300px", Height = "100%", Style = "position: absolute; Top:40px; right:2px;" });
                        StateHasChanged();
                    }
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
                    NavigationManager.NavigateTo("/setting-tabs");
                    break;
                case 4:
                    NavigationManager.NavigateTo("/on-progress-lab-test");
                    break;
                case 5:
                    NavigationManager.NavigateTo("/completed-lab-test");
                    break;
            }
            StateHasChanged();
        }
        protected async Task Login()
        {
       var result=     await _dialogService.OpenAsync<Login>("",
            new Dictionary<string, object>(),
            new DialogOptions { Width = "300px", Height = "100%", Style = "position: absolute; Top:40px; right:2px;" });
            StateHasChanged();
        }
        

        protected async Task Logout()
        {
            _authenticationService.Logout();
            IsAuthenticated = false;
            NavigationManager.NavigateTo("/lab-test",true);
            await  InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            _dialogService.Dispose();
            _labTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
            NavigationStateService.OnHeaderTabIndexChanged -= OnHeaderTabIndexChanged;
        }
    }
}