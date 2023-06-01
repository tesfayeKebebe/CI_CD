using Microsoft.AspNetCore.Components;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Pages.ViewModel;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.LabTests;

namespace Health.Mobile.Pages.LabTests
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
