using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Settings.Tests.Creates;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.LabTests;

namespace Web.UI.Pages.Settings.Tests;

public class LabTestContainerBase : ComponentBase
{
    [Inject] private ILabTestService _labTestService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    protected IList<LabTestDetail>? labTestDetails { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; }
    
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
        labTestDetails = await _labTestService.GetLabTest();
    }
    protected async Task OnAdd()
    {
        await _dialogService.OpenAsync<CreateTest>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }
}