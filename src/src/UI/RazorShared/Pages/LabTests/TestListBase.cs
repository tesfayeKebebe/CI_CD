using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Settings.Categories.Edits;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Models.LabTests;
using RazorShared.StateManagement;

namespace RazorShared.Pages.LabTests;

public class TestListBase : ComponentBase
{
   [Parameter] public LabTestCategory? Category { get; set; }
   [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
   [Parameter] public HashSet<string> SelectedLabTests { get; set; } = null!;
   [Inject] public  DialogService DialogService { get; set; }= null!;
    protected void OnSelect(LabTestCategory labTest, LabTestPriceDetail test)
   {
      foreach (var (_, cat) in LabTestCategoryStateServiceService.ViewModels )
      {
         if (cat.LabTests.ContainsKey(test.Id))
         {
            test.Selected = false;
            LabTestCategoryStateServiceService.RemoveLabTest(test);
            StateHasChanged();
            return;
         }
      }
      test.Selected = true;
      LabTestCategoryStateServiceService.AddLabTest(labTest,test);
      StateHasChanged();
   }

   protected void OnChange(LabTestCategory labTest, LabTestPriceDetail test)
   {
      foreach (var (_, cat) in LabTestCategoryStateServiceService.ViewModels )
      {
         if (cat.LabTests.ContainsKey(test.Id))
         {
            LabTestCategoryStateServiceService.RemoveLabTest(test);
            test.Selected = false;
            StateHasChanged();
            return;
         }
      }
      LabTestCategoryStateServiceService.AddLabTest(labTest,test);
      test.Selected = true;
      StateHasChanged();
   }

   protected async  Task OnShow(LabTestPriceDetail test)
   {

         var data = new LabTestDetailViewModel
         {
            Id = test.Id,
            Name = test.Name
         };
         await DialogService.OpenAsync<LabTestDetailInformation>("",
            new Dictionary<string, object>() {{"LabTestDetailViewModel", data}},
            new DialogOptions {Width = "50%", Height = "100%", Style = "position: absolute; Top:40px; "});
         StateHasChanged();


   }
}