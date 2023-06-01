using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Settings.Tests.Edits;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.LabTests;
namespace RazorShared.Pages.Settings.Tests;
public class LabTestListBase : ComponentBase
{
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Parameter]public IList<LabTestDetail>? LabTestDetails { get; set; } = null!;
    [Parameter] public EventCallback ChangesAsync { get; set; }
    protected async Task OnEdit(LabTestDetail test)
    {
        await DialogService.OpenAsync<EditTest>("",
            new Dictionary<string, object>() {{"Model", test}},
            new DialogOptions {Width = "50%", Height = "100%"});
        await  ChangesAsync.InvokeAsync(StateHasChanged);
    }

    protected async Task OnDelete(string id)
    {
        try
        {
            var confirm
                =  await DialogService.Confirm("Are you sure?", "Delete",
                    new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});
            if (confirm.Equals(true))
            {
                var result = await LabTestService.DeleteLabTest(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await ChangesAsync.InvokeAsync(StateHasChanged);
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
    
}