using Microsoft.AspNetCore.Components;
using RazorShared.Server.Api.Contracts;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Models.LabTests;

namespace RazorShared.Pages.LabTests
{
    public class LabTestDetailInformationBase  : ComponentBase
    {
        [Parameter] public LabTestDetailViewModel  LabTestDetailViewModel { get; set; } = new();
        [Inject] private ILabTestService LabTestService { get; set; } = null!;
        protected LabTestDetailById? LabTestDetail { get; set; }
        protected override async Task OnInitializedAsync()
        {
            LabTestDetail = await LabTestService.GetLabTestById(LabTestDetailViewModel.Id);
        }
    }
}
