using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Enums;
using RazorShared.Server.Models;
using RazorShared.Server.Models.SelectedTestDetails;
using RazorShared.Shared;

namespace RazorShared.Pages.LabTests;

public class CompletedLabTestBase: ComponentBase
{
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    protected List<SelectedTestDetail> SelectedTestDetails { get; set; } = new List<SelectedTestDetail>();
    protected readonly PeriodViewModel PeriodViewModel = new();
    protected override async Task OnInitializedAsync()
    {
        PeriodViewModel.Period = Period.Today;
        PeriodViewModel.From = DateTime.Now;
        PeriodViewModel.Until = DateTime.Now;
        await OnFilter();
    }

    protected async Task OnDone(SelectedTestDetail selected)
    {
        selected.IsCompleted = true;
        await DialogService.OpenAsync<TestResult>("Test Result",
            new Dictionary<string, object>() { {"selectedTestDetail",selected} },  
            new DialogOptions { Width = "50%", Height = "90%" });
        await OnFilter();
        StateHasChanged();
    }
    protected async Task OnPeriodSelectedAsync(PeriodSelectorEventArgs args)
    {
        PeriodViewModel.Period = args.Period;
        PeriodViewModel.From = args.From;
        PeriodViewModel.Until = args.Until;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task OnFilter()
    {
        var query = new SelectedTestDetailQuery
        {
            From = PeriodViewModel.From,
            To = PeriodViewModel.Until,
            TestStatus = TestStatus.Completed
        };
        SelectedTestDetails =    await SelectedTestDetailService.GetSelectedTestDetailsByDate(query);
        StateHasChanged();
    }


}