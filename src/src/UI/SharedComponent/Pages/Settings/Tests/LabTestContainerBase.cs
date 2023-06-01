using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.Tests.Creates;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.LabTests;

namespace SharedComponent.Pages.Settings.Tests;

public class LabTestContainerBase : ComponentBase
{
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected IList<LabTestDetail>? LabTestDetails { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
    protected  bool Expanded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
        IsSpinner=false;
    }
    protected void OnCategory()
    {
        Expanded = !Expanded;
    }

    protected async Task ChangesAsync()
    {
        await GetData();
    }
    private async Task GetData()
    {
        LabTestDetails = await LabTestService.GetLabTest();
    }
    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateTest>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "90%", Height = "90%", Draggable = true, Resizable = true,Style = "position: absolute; Top:70px; "});
        await GetData();
        StateHasChanged();
    }
}