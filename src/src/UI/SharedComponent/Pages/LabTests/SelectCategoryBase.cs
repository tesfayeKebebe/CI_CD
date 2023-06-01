using Microsoft.AspNetCore.Components;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.LabTests;

public class SelectCategoryBase : ComponentBase
{
    [Parameter] public Dictionary<string,LabTestPriceDetail> ViewModels { get; set; } = new Dictionary<string, LabTestPriceDetail>();
    [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
    [Inject] private  NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
    }

    protected void Remove(LabTestPriceDetail test)
   {
       LabTestCategoryStateServiceService.RemoveLabTest(test);
       NavigationManager.NavigateTo("/selected-lab-test");
       StateHasChanged();

   }
}