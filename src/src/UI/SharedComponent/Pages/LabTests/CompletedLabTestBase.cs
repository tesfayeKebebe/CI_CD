using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Models;
using SharedComponent.Server.Models.LabTestResults;
using SharedComponent.Server.Models.SelectedTestDetails;
using SharedComponent.Shared;

namespace SharedComponent.Pages.LabTests;

public class CompletedLabTestBase: ComponentBase
{
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private ITestResultService ResultService { get; set; } = null!;
    protected List<SelectedTestDetail> SelectedTestDetails { get; set; } = new List<SelectedTestDetail>();
    protected readonly PeriodViewModel PeriodViewModel = new();
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        PeriodViewModel.Period = Period.Today;
        PeriodViewModel.From = DateTime.Now;
        PeriodViewModel.Until = DateTime.Now;
        await OnFilter();
    }

    protected async Task OnDone(SelectedTestDetail selected)
    {
        var data = await GetTestResult(selected.TransactionNumber);
        data.TransactionNumber = selected.TransactionNumber;
        data.PatientName = selected.PatientName;
        data.PatientId = selected.PatientId;
        await DialogService.OpenAsync<TestResult>("Test Result",
            new Dictionary<string, object>() { {"Model",data} },  
            new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; " });
        await OnFilter();
        StateHasChanged();
    }
    private async Task<TestResultDetail> GetTestResult(string transactionNumber)
    {
        return  await ResultService.Get(transactionNumber);
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
        IsSpinner=true;
        var query = new SelectedTestDetailQuery
        {
            From = PeriodViewModel.From,
            To = PeriodViewModel.Until,
            TestStatus = TestStatus.Completed
        };
        SelectedTestDetails =    await SelectedTestDetailService.GetSelectedTestDetailsByDate(query);
        IsSpinner=false;
        StateHasChanged();
    }


}