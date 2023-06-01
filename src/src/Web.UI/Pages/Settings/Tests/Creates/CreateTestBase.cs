using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Categories;
using Web.UI.Server.Models.LabTests;
using Web.UI.Server.Models.Sample_Type;
using Web.UI.Server.Models.Tube_Type;

namespace Web.UI.Pages.Settings.Tests.Creates;
public  class CreateTestBase : ComponentBase
{
    protected LabTest model { get; set; } = new ();
     [Inject] private ILabTestService _labTestService{ get; set; }
     [Inject] private ISampleTypeService _sampleTypeService{ get; set; }
     [Inject] private ITubeTypeService _tubeTypeService{ get; set; }
     [Inject] private  NotificationService _notificationService { get; set; }
     [Inject] private  DialogService _dialogService { get; set; }
     [Inject] private ILabCategoryService labCategoryService { get; set; }
     protected IList<SampleTypeDetail?> SampleTypeDetails { get; set; }
     protected IList<TubeTypeDetail?> TubeDetails { get; set; }
     protected IList<CategoryDetail?> categoryDetails { get; set; }
     protected override async Task OnInitializedAsync()
     {
         await GetData();
     }
     private async Task GetData()
     {
         SampleTypeDetails = await _sampleTypeService.GetSampleType();
         TubeDetails = await _tubeTypeService.GetTubeType();
         categoryDetails = await labCategoryService.GetLabCategory();
     }
     protected async void OnSave()
    {
        try
        {
            var result= await _labTestService.CreateLabTest(model);
          _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
          _dialogService.Close();
        }
        catch (Exception e)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        model = new();
    }
}