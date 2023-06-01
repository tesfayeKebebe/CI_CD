using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using  RazorShared.Server.Models.Labs;

namespace RazorShared.Pages.Settings.Labs.Edits;

public class EditLabBase : ComponentBase
{
    [Parameter] public LabDetail Model { get; set; } = new();
    [Inject] private ILabService LabService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;

    protected async void OnSave()
    {
        try
        {
            var result = await LabService.UpdateLab(Model);
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