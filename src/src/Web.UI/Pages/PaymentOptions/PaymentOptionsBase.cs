using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.SelectedTestDetails;
using Web.UI.StateManagement;

namespace Web.UI.Pages.PaymentOptions;

public class PaymentOptionsBase : ComponentBase, IDisposable
{
    [Inject] private LabTestCategoryStateService _labTestCategoryStateServiceService { get; set; }
    [Inject] private  ISelectedTestDetailService _selectedTestDetailService { get; set; }
    [Inject] private  NotificationService _notificationService { get; set; }
    [Inject] private  NavigationManager NavigationManager { get; set; }
    protected double TotalPrice { get; set; }
    protected override async Task OnParametersSetAsync()
    {

        TotalPrice = _labTestCategoryStateServiceService.TotalPrice;
    }

    private void OnLabTestChanged()
    {
          
    }
    protected async Task OnSubmit()
    {
        try
        {
            var models = new List<SelectedTestDetailModel>();
            foreach (var (_, category) in _labTestCategoryStateServiceService.ViewModels)
            {
                foreach (var (__, test) in category.LabTests)
                {
                    var model = new SelectedTestDetailModel
                    {
                        Price = test.Price,
                        LabTestId = test.Id
                    };
                    models.Add(model);
                    _labTestCategoryStateServiceService.RemoveLabTest(test);
                }

            }
            var result= await _selectedTestDetailService.Create(models);
            _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
        }
        catch (Exception ex)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", ex.Message, 6000);
        }
        NavigationManager.NavigateTo("/lab-test");
        StateHasChanged();
    }

    protected void OnBack()
    {
        NavigationManager.NavigateTo("/selected-lab-test");
    }

    public void Dispose()
    {
        _labTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
    }
}