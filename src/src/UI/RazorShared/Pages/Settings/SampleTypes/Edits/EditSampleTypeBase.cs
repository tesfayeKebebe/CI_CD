using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Categories;
using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.Sample_Type;

namespace RazorShared.Pages.Settings.SampleTypes.Edits;

public class EditSampleTypeBase : ComponentBase
{
    [Parameter] public SampleTypeDetail Model { get; set; } = new();
    [Inject] private ISampleTypeService SampleTypeService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;



    protected async void OnSave()
    {
        try
        {
            var result = await SampleTypeService.UpdateSampleType(Model);
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