using Microsoft.AspNetCore.Components;
using Health.Mobile.Pages.ViewModel;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.SelectedTestDetails;

namespace Health.Mobile.Pages.LabTests;

public class SelectedLabTestDetailBase : ComponentBase
{
    [Parameter] public SelectedLabTestDetailViewModel SelectedLab { get; set; } = null!;
    protected IEnumerable<SelectedLabTestDetailByParentId>? SelectedLabTestDetails { get; set; }
    [Inject] private ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        SelectedLabTestDetails = await SelectedTestDetailService.GetLabTestById(SelectedLab.TransactionNumber);
    }
}