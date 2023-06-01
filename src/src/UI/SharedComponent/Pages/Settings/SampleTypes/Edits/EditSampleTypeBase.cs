using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Categories;
using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.Sample_Type;

namespace SharedComponent.Pages.Settings.SampleTypes.Edits;

public class EditSampleTypeBase : ComponentBase
{
    [Parameter] public SampleTypeDetail Model { get; set; } = new();
    [Inject] private ISampleTypeService SampleTypeService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    protected bool IsSpinner = false;
    protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
            var result = await SampleTypeService.UpdateSampleType(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            IsSpinner=false;
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