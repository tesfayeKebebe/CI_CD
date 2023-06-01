using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Settings.Tests.Creates;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models.LabTests;

namespace RazorShared.Pages.Settings.Tests;

public class LabTestContainerBase : ComponentBase
{
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected IList<LabTestDetail>? LabTestDetails { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;

    protected  bool Expanded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
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
            new DialogOptions {Width = "50%", Height = "100%"});
        await GetData();
        StateHasChanged();
    }
}