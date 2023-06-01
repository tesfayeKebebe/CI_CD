using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.TubeTypes.Creates;
using SharedComponent.Pages.Settings.TubeTypes.Edits;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Tube_Type;
namespace SharedComponent.Pages.Settings.TubeTypes;
public class TubeTypesBase : ComponentBase
{
    [Inject] private ITubeTypeService TubeTypeService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected List<TubeTypeDetail>? TubeDetails { get; set; }
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
        TubeDetails = await TubeTypeService.GetTubeType();
    }

    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateTubeType>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(TubeTypeDetail tubeType)
    {
        await DialogService.OpenAsync<EditTubeType>("",
            new Dictionary<string, object>() {{"Model", tubeType } },
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px;"});
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
                var result = await TubeTypeService.DeleteTubeType(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                IsSpinner=false;
                StateHasChanged();
            }
        }
        catch (Exception e)
        {
            IsSpinner=false;
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}