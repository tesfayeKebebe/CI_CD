using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Categories;
using Web.UI.Server.Models.Labs;

namespace Web.UI.Pages.Settings.Categories.Edits;

public class EditCategoryBase : ComponentBase
{
    [Parameter] public CategoryDetail model { get; set; } = new();
    [Inject] private ILabCategoryService _labCategoryService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    protected IList<LabDetail?> labs { get; set; }
    [Inject] private ILabService labService { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        labs=  await labService.GetLab();
    }

    protected async void OnSave()
    {
        try
        {
            var result = await _labCategoryService.UpdateLabCategory(model);
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