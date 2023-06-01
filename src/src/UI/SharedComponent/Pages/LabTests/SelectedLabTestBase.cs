using Microsoft.AspNetCore.Components;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Common.Services;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.LabTests
{
    public abstract class SelectedLabTestBase: ComponentBase, IDisposable
    {
        protected IDictionary<string, LabTestCategoryViewModel>
          ViewModels = new Dictionary<string, LabTestCategoryViewModel>();
        [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
        [Inject] private  NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private BearerAuthStateProvider BearerAuthStateProvider { get; set; } = null!;
        protected bool IsAuthenticated { get; set; }
        protected double TotalPrice { get; set; }
        protected int Count { get; set; }
        [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            Interceptor.RegisterEvent();
            ViewModels = LabTestCategoryStateServiceService.ViewModels;
            TotalPrice = LabTestCategoryStateServiceService.TotalPrice;
            Count = LabTestCategoryStateServiceService.SelectedLabTests.Count();
            LabTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
            var user =await BearerAuthStateProvider.GetAuthenticationStateAsync();
            if (user.User.Identity is {IsAuthenticated: true})
            {
                IsAuthenticated = true;
            }
        }

        private void OnLabTestChanged()
        {
          
        }
         protected void OnSubmit()
        {
            NavigationManager.NavigateTo("/payment-options");
        }

         protected void OnBack()
         {
             NavigationManager.NavigateTo("/lab-test");
         }

        public void Dispose()
        {
            LabTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
        }
    }
}
