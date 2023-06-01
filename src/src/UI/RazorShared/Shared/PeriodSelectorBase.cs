using Microsoft.AspNetCore.Components;
using RazorShared.Server.Enums;
using RazorShared.Server.Models;

namespace RazorShared.Shared;

public class PeriodSelectorBase : ComponentBase
{
    [Parameter] public Period Period { get; set; } = Period.CurrentMonth;
    [Parameter] public DateTime? From { get; set; }
    [Parameter] public DateTime? Until { get; set; }
    [Parameter] public EventCallback<PeriodSelectorEventArgs> OnPeriodSelected { get; set; }

    protected readonly KeyValueOf<Period, string>[] Periods = BuildPeriodList();
    protected bool IsDateDisable = true;
    
    protected override void OnParametersSet()
    {
        if (Period != Period.Individual)
        {
            SetDateTimesBySelectedPeriod();
        }
        
        StateHasChanged();
    }

    protected async Task OnPeriodSelectAsync()
    {
        SetDateTimesBySelectedPeriod();
        await InvokeAsync(StateHasChanged);
        await OnPeriodSelected.InvokeAsync(new PeriodSelectorEventArgs(Period, From, Until));
    }

    protected async Task OnTimeSelectAsync()
    {
        await InvokeAsync(StateHasChanged);
        await OnPeriodSelected.InvokeAsync(new PeriodSelectorEventArgs(Period, From, Until));
    }

    private void SetDateTimesBySelectedPeriod()
    {
        IsDateDisable = true;
       
        switch (Period)
        {
            case Period.CurrentMonth:
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                Until = null;
                break;
            case Period.LastMonth: 
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                Until = From.Value.AddMonths(1);
                break;
            case Period.Today:
                From = DateTime.Now.Date;
                Until = null;
                break;
            case Period.Yesterday:
                From = DateTime.Now.Date.AddDays(-1);
                Until = From.Value.AddDays(1);
                break;
            case Period.CurrentWeek:
                From = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek+1);
                Until = null;
                break; 
            case Period.LastWeek:
                From = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek-6);
                Until = From.Value.AddDays(7);
                break;
            case Period.Individual:
                From = DateTime.Now.Date;
                Until = null;
                IsDateDisable = false;
                break;

        }
    }
    
    private static KeyValueOf<Period, string>[] BuildPeriodList()
    {
        return new KeyValueOf<Period, string>[]
        {
            new(Period.CurrentMonth, "Current Month"),
            new(Period.LastMonth, "Last Month"),
            new(Period.Today, "Today"),
            new(Period.Yesterday, "Yesterday"),
            new(Period.CurrentWeek, "Current Week"),
            new(Period.LastWeek, "Last Week"),
            new(Period.Individual, "Individual")
        };
    }
}