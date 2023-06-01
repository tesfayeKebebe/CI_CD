using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Common.Services;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.Settings;

public class InfoTabBase : ComponentBase
, IDisposable
{
    protected int InfoTabIndex { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private NavigationStateService NavigationStateService { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected override Task OnParametersSetAsync()
    {
        Interceptor.RegisterEvent();
        NavigationStateService.OnInfoTabIndexChanged += OnInfoTabIndexChanged;
        InfoTabIndex = NavigationStateService.InfoTabIndex;
        Show(InfoTabIndex);
        return Task.CompletedTask;
    }



    private void OnInfoTabIndexChanged()
    {
        InfoTabIndex = NavigationStateService.InfoTabIndex;
        StateHasChanged();
    }
    protected void Show(int index)
    {
        NavigationStateService.SetInfoTabIndex(index);
        StateHasChanged();
        switch (index)
        {
            case 1:
                NavigationManager.NavigateTo("/info");
                break;
            case 2:
                NavigationManager.NavigateTo("/bank-account");
                break;
            
        }

    }

    public void Dispose()
    {
        NavigationStateService.OnHeaderTabIndexChanged -= OnInfoTabIndexChanged;
    }
}
