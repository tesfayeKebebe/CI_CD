using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Common.Services;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.Settings;

public class SettingTabBase : ComponentBase
, IDisposable
{
    protected int SettingTabIndex { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private NavigationStateService NavigationStateService { get; set; } = null!;
    
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected override Task OnParametersSetAsync()
    {
        Interceptor.RegisterEvent();
        NavigationStateService.OnSettingTabIndexChanged += OnSettingTabIndexChanged;
        SettingTabIndex = NavigationStateService.SettingTabIndex;
        Show(SettingTabIndex);
        return Task.CompletedTask;
    }



    private void OnSettingTabIndexChanged()
    {
        SettingTabIndex = NavigationStateService.SettingTabIndex;
        StateHasChanged();
    }
    protected void Show(int index)
    {
        NavigationStateService.SetSettingTabIndex(index);
        StateHasChanged();
        switch (index)
        {
            case 1:
                NavigationManager.NavigateTo("/lab-list");
                break;
            case 2:
                NavigationManager.NavigateTo("/lab-categories");
                break;
            case 3:
                NavigationManager.NavigateTo("/sample-type");
                break;
            case 4:
                NavigationManager.NavigateTo("/tube-type");
                break;
            case 5:
                NavigationManager.NavigateTo("/lab-test-list");
                break;
            case 6:
                NavigationManager.NavigateTo("/Test-price-list");
                break;
        }

    }

    public void Dispose()
    {
        NavigationStateService.OnSettingTabIndexChanged -= OnSettingTabIndexChanged;
    }
}
