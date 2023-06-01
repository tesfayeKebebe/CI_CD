using Microsoft.AspNetCore.Components;
using Web.UI.StateManagement;

namespace Web.UI.Shared;

public class FooterBase : ComponentBase, IDisposable
{
    [Inject] private LabTestCategoryStateService _labTestCategoryStateServiceService { get; set; } = null!;
    [Inject]  private NavigationManager NavigationManager { get; set; }
    [Inject] private  NavigationStateService NavigationStateService { get; set; }
    protected int FooterTabIndex { get; set; }
    [Parameter]  public int TotalItem { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        NavigationStateService.OnFooterTabIndexChanged += OnFooterTabIndexChanged;
        FooterTabIndex = NavigationStateService.FooterTabIndex;
        Show(FooterTabIndex);
        _labTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
    }

    private void OnFooterTabIndexChanged()
    {
    }

    private void OnLabTestChanged()
    {
        TotalItem = _labTestCategoryStateServiceService.TotalItem;
        if (TotalItem == 0)
        {
            NavigationManager.NavigateTo("/lab-test");
        }
        StateHasChanged();
    }
    protected void Show(int index)
    {
        NavigationStateService.SetFooterTabIndex(index);
        StateHasChanged();
        switch (index)
        {
            case 1:
                NavigationManager.NavigateTo("/selected-lab-test");
                break;
        }
           
    }
        
    public void Dispose()
    {
        _labTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
        NavigationStateService.OnFooterTabIndexChanged -= OnFooterTabIndexChanged;
    }
}