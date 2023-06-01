using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Settings.SampleTypes.Creates;
using Web.UI.Pages.Settings.SampleTypes.Edits;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Sample_Type;
namespace Web.UI.Pages.Settings.SampleTypes;
public class TubeTypesBase : ComponentBase
{
    [Inject] private ISampleTypeService _sampleTypeService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    protected IList<SampleTypeDetail>? SampleTypeDetails { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
    }

    private async Task GetData()
    {
        SampleTypeDetails = await _sampleTypeService.GetSampleType();
    }

    protected async Task OnAdd()
    {
        await _dialogService.OpenAsync<CreateSampleType>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(SampleTypeDetail sample)
    {
        await _dialogService.OpenAsync<EditSampleType>("",
            new Dictionary<string, object>() {{"model", sample}},
            new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnDelete(string id)
    {
        try
        {
            var result = await _sampleTypeService.DeleteSampleType(id);
            _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            await GetData();
            StateHasChanged();
        }
        catch (Exception e)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}