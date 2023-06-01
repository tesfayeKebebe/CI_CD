using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Categories;
using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.Sample_Type;
using RazorShared.Server.Models.Tube_Type;

namespace RazorShared.Pages.Settings.TubeTypes.Creates;
public  class CreateTubeTypeBase : ComponentBase
{
    protected TubeType Model { get; set; } = new ();
     [Inject] private ITubeTypeService TubeTypeService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected async void OnSave()
    {
        try
        {
          var result= await TubeTypeService.CreateTubeType(Model);
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
        Model = new TubeType();
    }
}