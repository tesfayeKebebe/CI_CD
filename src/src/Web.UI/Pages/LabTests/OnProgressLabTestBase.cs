using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Enums;
using Web.UI.Server.Models.SelectedTestDetails;

namespace Web.UI.Pages.LabTests;

public class OnProgressLabTestBase : ComponentBase
{
    [Inject] private  ISelectedTestDetailService _selectedTestDetailService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    protected IEnumerable<SelectedTestDetail> _selectedTestDetails { get; set; } = new Collection<SelectedTestDetail>();
    protected override async Task OnInitializedAsync()
    {
        await GetData();
    }

    protected async Task OnDone(SelectedTestDetail selected)
    {
        await _dialogService.OpenAsync<TestResult>("",
            new Dictionary<string, object>() { {"selectedTestDetail",selected} },  
            new DialogOptions { Width = "50%", Height = "100%", Style = "position: absolute; Top:40px; " });
        await GetData();
        StateHasChanged();
    }

    private async Task GetData()
    {
        _selectedTestDetails = await _selectedTestDetailService.GetSelectedTestDetails(TestStatus.OnProgress);
    }
}