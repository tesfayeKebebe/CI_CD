using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Settings.TubeTypes.Creates;
using RazorShared.Pages.Settings.TubeTypes.Edits;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models.Tube_Type;
namespace RazorShared.Pages.Settings.TubeTypes;
public class TubeTypesBase : ComponentBase
{
    [Inject] private ITubeTypeService TubeTypeService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected List<TubeTypeDetail>? TubeDetails { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
    }

    private async Task GetData()
    {
        TubeDetails = await TubeTypeService.GetTubeType();
    }

    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateTubeType>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "570px"});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(TubeTypeDetail tubeType)
    {
        await DialogService.OpenAsync<EditTubeType>("",
            new Dictionary<string, object>() {{"Model", tubeType } },
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
                var result = await TubeTypeService.DeleteTubeType(id);
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