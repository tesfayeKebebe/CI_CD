using Microsoft.AspNetCore.Components;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.SelectedTestDetails;

namespace SharedComponent.Pages.LabTests;

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