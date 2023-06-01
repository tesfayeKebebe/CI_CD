using Health.Mobile.StateManagement;
using Microsoft.AspNetCore.Components;
using Health.Mobile.Server.Common.Services;


namespace Health.Mobile.Shared;

public class FooterBase : ComponentBase, IDisposable
{
    [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
    [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private  NavigationStateService NavigationStateService { get; set; } = null!;
    [Inject] private BearerAuthStateProvider BearerAuthStateProvider { get; set; } = null!;
    protected bool IsAuthenticated { get; set; }
    protected int FooterTabIndex { get; set; }
    [Parameter]  public int TotalItem { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        var user =await BearerAuthStateProvider.GetAuthenticationStateAsync();
        if (user.User.Identity is {IsAuthenticated: true})
        {
            IsAuthenticated = true;
        }
        NavigationStateService.OnFooterTabIndexChanged += OnFooterTabIndexChanged;
        FooterTabIndex = NavigationStateService.FooterTabIndex;
        Show(FooterTabIndex);
        LabTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
        StateHasChanged();

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
            case 2:
                NavigationManager.NavigateTo("/home");
                break;
        }
           
    }
        
    public void Dispose()
    {
        LabTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
        NavigationStateService.OnFooterTabIndexChanged -= OnFooterTabIndexChanged;
    }
}