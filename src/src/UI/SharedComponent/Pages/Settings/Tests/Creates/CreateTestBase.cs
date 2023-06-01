using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Categories;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.Server.Models.Sample_Type;
using SharedComponent.Server.Models.Tube_Type;

namespace SharedComponent.Pages.Settings.Tests.Creates;
public  class CreateTestBase : ComponentBase, IDisposable
{
    protected LabTest Model { get; set; } = new ();
     [Inject] private ILabTestService LabTestService{ get; set; } = null!;
    [Inject] private ISampleTypeService SampleTypeService{ get; set; } = null!;
    [Inject] private ITubeTypeService TubeTypeService{ get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    [Inject] private ILabCategoryService LabCategoryService { get; set; } = null!;
    protected List<SampleTypeDetail>? SampleTypeDetails { get; set; }
     protected List<TubeTypeDetail>? TubeDetails { get; set; }
     protected List<CategoryDetail>? CategoryDetails { get; set; }
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
     {
         await GetData();
        IsSpinner=false;
     }
     private async Task GetData()
     {
         SampleTypeDetails = await SampleTypeService.GetSampleType();
         TubeDetails = await TubeTypeService.GetTubeType();
         CategoryDetails = await LabCategoryService.GetLabCategory();
     }
     protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
            var result= await LabTestService.CreateLabTest(Model);
          NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            IsSpinner=false;

        }
        catch (Exception e)
        {
            IsSpinner=false;
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        Model = new();
    }

    public void Dispose()
    {
  
        DialogService.Dispose();
    }
}