using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Enums;
using RazorShared.Server.Models.SelectedTestDetails;

namespace RazorShared.Pages.LabTests.Drafts;

public class DraftBase: ComponentBase
{
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    protected List<SelectedTestDetail> SelectedTestDetails { get; set; } = new List<SelectedTestDetail>();
    protected override async Task OnInitializedAsync()
    {
        await GetData();
    }
    protected async Task OnDone(SelectedTestDetail selected)
    {
        var model = new SelectedLabTestDetailViewModel
        {
            Name = selected.PatientName,
            TransactionNumber = selected.TransactionNumber
        };
        await DialogService.OpenAsync<ApproveDraft>("",
            new Dictionary<string, object>() { {"SelectedTestDetail",model} },  
            new DialogOptions { Width = "40%", Height = "50px", Style = "position: absolute; Top:40px;" });
        await GetData();
        StateHasChanged();
    }


    private async Task GetData()
    {
        SelectedTestDetails = await SelectedTestDetailService.GetSelectedTestDetails(TestStatus.Draft);
    }
    
}