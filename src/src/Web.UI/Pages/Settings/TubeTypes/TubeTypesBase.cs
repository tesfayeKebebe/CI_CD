using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Settings.TubeTypes.Creates;
using Web.UI.Pages.Settings.TubeTypes.Edits;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Tube_Type;
namespace Web.UI.Pages.Settings.TubeTypes;
public class TubeTypesBase : ComponentBase
{
    [Inject] private ITubeTypeService _tubeTypeService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    protected IList<TubeTypeDetail>? TubeDetails { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
    }

    private async Task GetData()
    {
        TubeDetails = await _tubeTypeService.GetTubeType();
    }

    protected async Task OnAdd()
    {
        await _dialogService.OpenAsync<CreateTubeType>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(TubeTypeDetail tubeType)
    {
        await _dialogService.OpenAsync<EditTubeType>("",
            new Dictionary<string, object>() {{"model", tubeType } },
            new DialogOptions {Width = "300px", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnDelete(string id)
    {
        try
        {
            var result = await _tubeTypeService.DeleteTubeType(id);
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