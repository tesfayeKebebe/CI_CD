using Microsoft.AspNetCore.Components;
using Web.UI.Pages.ViewModel;
using Web.UI.Server.Models.LabTests;
using Web.UI.StateManagement;

namespace Web.UI.Pages.LabTests;

public class SelectCategoryBase : ComponentBase
{
    [Parameter] public Dictionary<string,LabTestPriceDetail> ViewModels { get; set; } = new Dictionary<string, LabTestPriceDetail>();
    [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; }
    [Inject] private  NavigationManager NavigationManager { get; set; }
   protected void Remove(LabTestPriceDetail test)
   {
       LabTestCategoryStateServiceService.RemoveLabTest(test);
       NavigationManager.NavigateTo("/selected-lab-test");
       StateHasChanged();

   }
}