using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Categories;
using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.Sample_Type;
using SharedComponent.Server.Models.Tube_Type;

namespace SharedComponent.Pages.Settings.TubeTypes.Creates;
public  class CreateTubeTypeBase : ComponentBase
{
    protected TubeType Model { get; set; } = new ();
     [Inject] private ITubeTypeService TubeTypeService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected bool IsSpinner = false;
    protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
          var result= await TubeTypeService.CreateTubeType(Model);
          NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            IsSpinner=false;
          DialogService.Close();
        }
        catch (Exception e)
        {
            IsSpinner=false;
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

    }

    protected void OnReset()
    {
        Model = new TubeType();
    }
}