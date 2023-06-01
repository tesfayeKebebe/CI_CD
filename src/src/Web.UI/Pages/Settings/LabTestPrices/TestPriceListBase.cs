using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Settings.LabTestPrices.Creates;
using Web.UI.Pages.Settings.LabTestPrices.Edits;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.TestPrices;
namespace Web.UI.Pages.Settings.LabTestPrices;
public class TestPriceListBase : ComponentBase
{
    [Inject] private ITestPriceService _testPriceService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    protected IList<TestPriceDetail>? testPriceDetails { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; }
    protected IList<TestPriceDetail> Originaldata { get; set; }
    protected string? _search { get; set; }
    protected bool _clearEnabled { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
    }
    protected void OnInputChanged(ChangeEventArgs obj)
    {
        _clearEnabled = !string.IsNullOrEmpty(obj.Value?.ToString());
        _search = obj.Value?.ToString();
        SearchAsync();
        StateHasChanged();
    }
    protected void Clear()
    {
        _search = null;
        _clearEnabled = false;
        testPriceDetails = Originaldata.ToList();
    }

    private void SearchAsync()
    {
        if(_search==null) return;
        testPriceDetails = new List<TestPriceDetail>();
        foreach (var testPrice in Originaldata.Where(x=>_search != null && x.LabTest.ToLower().Contains(_search.ToLower() )).ToList().Select(price => new TestPriceDetail()
                 {
                     Id = price.Id,
                     Price = price.Price,
                     IsActive = price.IsActive,
                     LabTest = price.LabTest,
                     LabTestId = price.LabTestId
                 }))
        {
            testPriceDetails.Add(testPrice);
        }
    }
    private async Task GetData()
    {
        testPriceDetails = await _testPriceService.GetTestPrice();
        Originaldata = testPriceDetails;
    }

    protected async Task OnAdd()
    {
        await _dialogService.OpenAsync<CreateTestPrice>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "100%", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(TestPriceDetail test)
    {
        await _dialogService.OpenAsync<EditTestPrice>("",
            new Dictionary<string, object>() {{"model", test}},
            new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnDelete(string id)
    {
        try
        {
            var result = await _testPriceService.DeleteTestPrice(id);
            _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            await GetData();
            StateHasChanged();
        }
        catch (Exception e)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}