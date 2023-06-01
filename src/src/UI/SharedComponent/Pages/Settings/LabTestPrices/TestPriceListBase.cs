using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.LabTestPrices.Creates;
using SharedComponent.Pages.Settings.LabTestPrices.Edits;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.TestPrices;
namespace SharedComponent.Pages.Settings.LabTestPrices;
public class TestPriceListBase : ComponentBase
{
    [Inject] private ITestPriceService TestPriceService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected IList<TestPriceDetail>? TestPriceDetails { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    private IList<TestPriceDetail> OriginalData { get; set; } = null!;
    protected string? Search { get; set; }
    protected bool ClearEnabled { get; set; }
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
        IsSpinner=false;
    }
    protected void OnInputChanged(ChangeEventArgs obj)
    {
        ClearEnabled = !string.IsNullOrEmpty(obj.Value?.ToString());
        Search = obj.Value?.ToString();
        SearchAsync();
        StateHasChanged();
    }
    protected void Clear()
    {
        Search = null;
        ClearEnabled = false;
        TestPriceDetails = OriginalData.ToList();
    }

    private void SearchAsync()
    {
        if(Search==null) return;
        TestPriceDetails = new List<TestPriceDetail>();
        foreach (var testPrice in OriginalData.Where(x=>Search != null && x.LabTest.ToLower().Contains(Search.ToLower() )).ToList().Select(price => new TestPriceDetail()
                 {
                     Id = price.Id,
                     Price = price.Price,
                     IsActive = price.IsActive,
                     LabTest = price.LabTest,
                     LabTestId = price.LabTestId
                 }))
        {
            TestPriceDetails.Add(testPrice);
        }
    }
    private async Task GetData()
    {
        TestPriceDetails = await TestPriceService.GetTestPrice();
        OriginalData = TestPriceDetails;
    }

    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateTestPrice>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(TestPriceDetail test)
    {
        await DialogService.OpenAsync<EditTestPrice>("",
            new Dictionary<string, object>() {{"Model", test}},
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true, Style = "position: absolute; Top:80px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnDelete(string id)
    {
        try
        {
            var confirm
                =  await DialogService.Confirm("Are you sure?", "Delete",
                    new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});
            if (confirm.Equals(true))
            {
                IsSpinner=true;
                var result = await TestPriceService.DeleteTestPrice(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                IsSpinner=false;
                StateHasChanged();
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}