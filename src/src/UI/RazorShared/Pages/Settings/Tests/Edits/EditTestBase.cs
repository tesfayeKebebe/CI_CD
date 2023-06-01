using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Categories;
using RazorShared.Server.Models.LabTests;
using RazorShared.Server.Models.Sample_Type;
using RazorShared.Server.Models.Tube_Type;

namespace RazorShared.Pages.Settings.Tests.Edits;

public class EditTestBase : ComponentBase
{
    [Parameter] public LabTestDetail Model { get; set; } = new();
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private ISampleTypeService SampleTypeService{ get; set; } = null!;
    [Inject] private ITubeTypeService TubeTypeService{ get; set; } = null!;
    [Inject] private ILabCategoryService LabCategoryService { get; set; } = null!;
    protected List<SampleTypeDetail>? SampleTypeDetails { get; set; } 
    protected List<TubeTypeDetail>? TubeDetails { get; set; }
    protected List<CategoryDetail>? CategoryDetails { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await GetData();
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
            var result = await LabTestService.UpdateLabTest(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            DialogService.Close();
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        Model.Name = null;
    }
}