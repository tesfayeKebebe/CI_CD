using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.Labs;

namespace RazorShared.Pages.Settings.Labs.Creates;
public class CreateBase : ComponentBase
{
    protected Lab Model { get; set; } = new ();
     [Inject] private ILabService LabService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  DialogService DialogService { get; set; } = null!;
    protected async void OnSave()
    {
        try
        {
          var result= await LabService.CreateLab(Model);
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
        Model = new Lab();
    }
}