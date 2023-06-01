using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Web.UI.Pages.ViewModel;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Models.LabTests;
using Web.UI.StateManagement;

namespace Web.UI.Pages.LabTests
{
    public class SelectedLabtTestBase: ComponentBase, IDisposable
    {
        protected IDictionary<string, LabTestCategoryViewModel>
          ViewModels = new Dictionary<string, LabTestCategoryViewModel>();
        [Inject] private LabTestCategoryStateService _labTestCategoryStateServiceService { get; set; }
        [Inject] private  NavigationManager NavigationManager { get; set; }
        [Inject] private BearerAuthStateProvider _bearerAuthStateProvider { get; set; }
        protected bool IsAuthenticated { get; set; }
        protected double TotalPrice { get; set; }
        protected int Count { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            ViewModels = _labTestCategoryStateServiceService.ViewModels;
            TotalPrice = _labTestCategoryStateServiceService.TotalPrice;
            Count = _labTestCategoryStateServiceService.SelectedLabTests.Count();
            _labTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
            var user =await _bearerAuthStateProvider.GetAuthenticationStateAsync();
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
            _labTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
        }
    }
}
