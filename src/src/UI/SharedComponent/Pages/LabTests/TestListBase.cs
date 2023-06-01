using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.Categories.Edits;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.LabTests;

public class TestListBase : ComponentBase
{
    [Parameter] public IList<LabTestCategory>? Categories { get; set; }
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
            new DialogOptions {Width = "60%", Height = "100%", Draggable = true, Resizable = true, CloseDialogOnOverlayClick = true, Style = "position: absolute; Top:80px; "});
         StateHasChanged();


   }
}