using RazorShared.StateManagement;
using Microsoft.AspNetCore.Components;


namespace RazorShared.Shared;

public class FooterBase : ComponentBase, IDisposable
{
    [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
    [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private  NavigationStateService NavigationStateService { get; set; } = null!;
    protected int FooterTabIndex { get; set; }
    [Parameter]  public int TotalItem { get; set; }
    protected override Task OnParametersSetAsync()
    {
        NavigationStateService.OnFooterTabIndexChanged += OnFooterTabIndexChanged;
        FooterTabIndex = NavigationStateService.FooterTabIndex;
        Show(FooterTabIndex);
        LabTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
        return Task.CompletedTask;
    }

    private void OnFooterTabIndexChanged()
    {
    }

    private void OnLabTestChanged()
    {
        TotalItem = LabTestCategoryStateServiceService.TotalItem;
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
        LabTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
        NavigationStateService.OnFooterTabIndexChanged -= OnFooterTabIndexChanged;
    }
}