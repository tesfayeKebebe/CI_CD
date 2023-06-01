using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Settings.SampleTypes.Creates;
using RazorShared.Pages.Settings.SampleTypes.Edits;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models.Sample_Type;
namespace RazorShared.Pages.Settings.SampleTypes;
public class SampleTypesBase : ComponentBase
{
    [Inject] private ISampleTypeService SampleTypeService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected List<SampleTypeDetail>? SampleTypeDetails { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
    }

    private async Task GetData()
    {
        SampleTypeDetails = await SampleTypeService.GetSampleType();
    }

    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateSampleType>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "570px"});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(SampleTypeDetail sample)
    {
        await DialogService.OpenAsync<EditSampleType>("",
            new Dictionary<string, object>() {{"Model", sample}},
            new DialogOptions {Width = "50%", Height = "570px"});
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
                var result = await SampleTypeService.DeleteSampleType(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                StateHasChanged();
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}