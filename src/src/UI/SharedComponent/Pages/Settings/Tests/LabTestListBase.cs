using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.Tests.Edits;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.LabTests;
namespace SharedComponent.Pages.Settings.Tests;
public class LabTestListBase : ComponentBase
{
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Parameter]public IList<LabTestDetail>? LabTestDetails { get; set; } = null!;
    [Parameter] public EventCallback ChangesAsync { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = false;
    protected async Task OnEdit(LabTestDetail test)
    {
        Interceptor.RegisterEvent();
        await DialogService.OpenAsync<EditTest>("",
            new Dictionary<string, object>() {{"Model", test}},
            new DialogOptions {Width = "90%", Height = "90%", Draggable = true, Resizable = true,Style = "position: absolute; Top:70px; "});
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
                IsSpinner=true;
                var result = await LabTestService.DeleteLabTest(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                IsSpinner=false;
                await ChangesAsync.InvokeAsync(StateHasChanged);
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
    
}