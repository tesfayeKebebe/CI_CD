using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Categories;
using Web.UI.Server.Models.Labs;
using Web.UI.Server.Models.Sample_Type;
using Web.UI.Server.Models.Tube_Type;

namespace Web.UI.Pages.Settings.TubeTypes.Edits;

public class EditTubeTypeBase : ComponentBase
{
    [Parameter] public TubeTypeDetail model { get; set; } = new();
    [Inject] private ITubeTypeService _tubeTypeService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }

    protected async void OnSave()
    {
        try
        {
            var result = await _tubeTypeService.UpdateTubeType(model);
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
        model.Name = null;
    }
}