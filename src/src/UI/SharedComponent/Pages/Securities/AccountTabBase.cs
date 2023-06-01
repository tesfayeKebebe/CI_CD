using Microsoft.AspNetCore.Components;
using SharedComponent.Server.Common.Services;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.Securities;

public class AccountTabBase : ComponentBase,IDisposable
{
    protected int SettingTabIndex { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private NavigationStateService NavigationStateService { get; set; } = null!;

    protected bool IsAuthenticated { get; set; }
    protected bool IsAdmin { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected override async Task OnParametersSetAsync()
    {
        Interceptor.RegisterEvent();
        NavigationStateService.OnHeaderTabIndexChanged += OnHeaderTabIndexChanged;
        SettingTabIndex = NavigationStateService.SettingTabIndex;
       await Show(SettingTabIndex);
       
    }



    private void OnHeaderTabIndexChanged()
    {
        SettingTabIndex = NavigationStateService.SettingTabIndex;
        StateHasChanged();
    }
    protected async Task Show(int index)
    {
        NavigationStateService.SetSettingTabIndex(index);
        StateHasChanged();
        switch (index)
        {
            case 1:
                NavigationManager.NavigateTo("/user-list");
                break;
            case 2:
                NavigationManager.NavigateTo("/user-branch");
                break;
            case 3:
                NavigationManager.NavigateTo("/user-assign");
                break;
  
        }

    }

    public void Dispose()
    {
        NavigationStateService.OnHeaderTabIndexChanged -= OnHeaderTabIndexChanged;
    }
}