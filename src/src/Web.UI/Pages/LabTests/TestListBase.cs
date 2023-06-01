using Microsoft.AspNetCore.Components;
using Web.UI.Server.Models.LabTests;
using Web.UI.StateManagement;

namespace Web.UI.Pages.LabTests;

public class TestListBase : ComponentBase
{
   [Parameter] public LabTestCategory? Category { get; set; }
   [Inject] private LabTestCategoryStateService _labTestCategoryStateServiceService { get; set; } = null!;
   [Parameter] public HashSet<string> SelectedLabTests { get; set; }
   protected void OnSelect(LabTestCategory labTest, LabTestPriceDetail test)
   {
      foreach (var (_, cat) in _labTestCategoryStateServiceService.ViewModels )
      {
         if (cat.LabTests.ContainsKey(test.Id))
         {
            test.Selected = false;
            _labTestCategoryStateServiceService.RemoveLabTest(test);
            StateHasChanged();
            return;
         }
      }
      test.Selected = true;
      _labTestCategoryStateServiceService.AddLabTest(labTest,test);
      StateHasChanged();
   }

   protected void OnChange(LabTestCategory labTest, LabTestPriceDetail test)
   {
      foreach (var (_, cat) in _labTestCategoryStateServiceService.ViewModels )
      {
         if (cat.LabTests.ContainsKey(test.Id))
         {
            _labTestCategoryStateServiceService.RemoveLabTest(test);
            test.Selected = false;
            StateHasChanged();
            return;
         }
      }
      _labTestCategoryStateServiceService.AddLabTest(labTest,test);
      test.Selected = true;
      StateHasChanged();
   }
}