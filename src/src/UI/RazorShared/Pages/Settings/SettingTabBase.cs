using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.StateManagement;

namespace RazorShared.Pages.Settings;

public class SettingTabBase : ComponentBase
, IDisposable
{
    protected int SettingTabIndex { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private NavigationStateService NavigationStateService { get; set; } = null!;

    protected bool IsAuthenticated { get; set; }
    protected bool IsAdmin { get; set; }
    
    protected override Task OnParametersSetAsync()
    {
        NavigationStateService.OnHeaderTabIndexChanged += OnHeaderTabIndexChanged;
        SettingTabIndex = NavigationStateService.SettingTabIndex;
        Show(SettingTabIndex);
        return Task.CompletedTask;
    }



    private void OnHeaderTabIndexChanged()
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
        NavigationStateService.OnHeaderTabIndexChanged -= OnHeaderTabIndexChanged;
    }
}
