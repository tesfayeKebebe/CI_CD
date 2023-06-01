using Microsoft.AspNetCore.Components;
using RazorShared.StateManagement;

namespace RazorShared.Pages.Securities;

public class AccountTabBase : ComponentBase,IDisposable
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