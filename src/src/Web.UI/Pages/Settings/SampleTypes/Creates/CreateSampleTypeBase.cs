using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Categories;
using Web.UI.Server.Models.Labs;
using Web.UI.Server.Models.Sample_Type;

namespace Web.UI.Pages.Settings.SampleTypes.Creates;
public  class CreateTubeTypeBase : ComponentBase
{
    protected SampleType model { get; set; } = new ();
     [Inject] private ISampleTypeService _sampleTypeService { get; set; }
     [Inject] private  NotificationService _notificationService { get; set; }
     [Inject] private  DialogService _dialogService { get; set; }
  
     protected async void OnSave()
    {
        try
        {
          var result= await _sampleTypeService.CreateSampleType(model);
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
        model = new();
    }
}