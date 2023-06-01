using Microsoft.AspNetCore.Components;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Models.SelectedTestDetails;

namespace RazorShared.Pages.LabTests;

public class SelectedLabTestDetailBase : ComponentBase
{
    [Parameter] public SelectedLabTestDetailViewModel SelectedLab { get; set; } = null!;
    protected IEnumerable<SelectedLabTestDetailByParentId>? SelectedLabTestDetails { get; set; }
    [Inject] private ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        SelectedLabTestDetails = await SelectedTestDetailService.GetLabTestById(SelectedLab.TransactionNumber);
    }
}