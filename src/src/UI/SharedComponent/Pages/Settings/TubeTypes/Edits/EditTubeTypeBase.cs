using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Categories;
using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.Sample_Type;
using SharedComponent.Server.Models.Tube_Type;

namespace SharedComponent.Pages.Settings.TubeTypes.Edits;

public class EditTubeTypeBase : ComponentBase
{
    [Parameter] public TubeTypeDetail Model { get; set; } = new();
    [Inject] private ITubeTypeService TubeTypeService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = false;
    protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
            Interceptor.RegisterEvent();
            var result = await TubeTypeService.UpdateTubeType(Model);
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