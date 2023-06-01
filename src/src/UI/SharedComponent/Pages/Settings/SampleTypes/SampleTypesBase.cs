using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.SampleTypes.Creates;
using SharedComponent.Pages.Settings.SampleTypes.Edits;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Sample_Type;
namespace SharedComponent.Pages.Settings.SampleTypes;
public class SampleTypesBase : ComponentBase
{
    [Inject] private ISampleTypeService SampleTypeService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected List<SampleTypeDetail>? SampleTypeDetails { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
        IsSpinner=false;
    }

    private async Task GetData()
    {
        SampleTypeDetails = await SampleTypeService.GetSampleType();
    }

    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateSampleType>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px;"});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(SampleTypeDetail sample)
    {
        await DialogService.OpenAsync<EditSampleType>("",
            new Dictionary<string, object>() {{"Model", sample}},
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; "});
        await GetData();
        StateHasChanged();
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
                var result = await SampleTypeService.DeleteSampleType(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                IsSpinner=false;
                StateHasChanged();
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}