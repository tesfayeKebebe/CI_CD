using Microsoft.AspNetCore.Components;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.LabTests;

namespace SharedComponent.Pages.LabTests
{
    public class LabTestDetailInformationBase  : ComponentBase
    {
        [Parameter] public LabTestDetailViewModel  LabTestDetailViewModel { get; set; } = new();
        [Inject] private ILabTestService LabTestService { get; set; } = null!;
        protected LabTestDetailById? LabTestDetail { get; set; }
        [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
            LabTestDetail = await LabTestService.GetLabTestById(LabTestDetailViewModel.Id);
        }
    }
}
