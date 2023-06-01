using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Categories;
using SharedComponent.Server.Models.Labs;

namespace SharedComponent.Pages.Settings.Categories.Edits;

public class EditCategoryBase : ComponentBase
{
    [Parameter] public CategoryDetail Model { get; set; } = new();
    [Inject] private ILabCategoryService LabCategoryService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    protected List<LabDetail>? Labs { get; set; }
    [Inject] private ILabService LabService { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnParametersSetAsync()
    {
        Labs=  await LabService.GetLab();
        IsSpinner=false;
    }

    protected async void OnSave()
    {
        try
        {
            IsSpinner=true;
            var result = await LabCategoryService.UpdateLabCategory(Model);
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