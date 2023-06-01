using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Settings.Tests.Edits;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.LabTests;
namespace Web.UI.Pages.Settings.Tests;
public class LabTestListBase : ComponentBase
{
    [Inject] private ILabTestService _labTestService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    [Parameter]public IList<LabTestDetail>? labTestDetails { get; set; }
    [Parameter] public EventCallback ChangesAsync { get; set; }
    protected async Task OnEdit(LabTestDetail test)
    {
        await _dialogService.OpenAsync<EditTest>("",
            new Dictionary<string, object>() {{"model", test}},
            new DialogOptions {Width = "50%", Height = "100%", Style = "position: absolute; Top:40px; "});
        await  ChangesAsync.InvokeAsync(StateHasChanged);
    }

    protected async Task OnDelete(string id)
    {
        try
        {
            var result = await _labTestService.DeleteLabTest(id);
            _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            await  ChangesAsync.InvokeAsync(StateHasChanged);
        }
        catch (Exception e)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
    
}