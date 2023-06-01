using System.Text;
using Microsoft.AspNetCore.Components;
using Radzen;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Api.Impl;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Enums;
using Health.Mobile.Server.Models.LabTests;
using Health.Mobile.Server.Models.PatientFiles;
using Health.Mobile.Server.Models.SelectedTestDetails;

namespace Health.Mobile.Pages.LabTests.TemporaryFileUploads;

public class AddingPatientBase: ComponentBase
{
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    [Inject] private ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService { get; set; }= null!;
    [Inject] private  HttpClient _httpClient { get; set; } = null!;
    [Inject] private  ApiSetting _apiSetting{ get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    private SessionStore SessionStore = new();
    protected PatientFileDetail? Data = new();
    [Parameter] public  List<PatientFileDetail> PatientFileDetails { get; set; } = null!;
    [Inject] private  IPatientFileService  PatientFileService { get; set; } = null!;
    protected SelectedTestDetailModel Model { get; set; } = new ();
    protected double? TotalPrice = 0;
    protected List<LabTestPriceDetail> LabTestPriceDetails { get; set; } = new List<LabTestPriceDetail>();
    protected Dictionary<string, LabTestPriceDetail?> AddedLabTestPriceDetails = new Dictionary<string, LabTestPriceDetail?>();
    protected override async Task OnInitializedAsync()
    {
        Data=  PatientFileDetails.FirstOrDefault();
        LabTestPriceDetails=  await LabTestService.GetLabTestsDropDown();
        SessionStore = await SessionStoreService.Get();
    }

    protected async Task OnRemove(string id)
    {
        if (AddedLabTestPriceDetails.ContainsKey(id))
        {
            AddedLabTestPriceDetails.Remove(id);
            StateHasChanged();
        }
    }
    protected async Task OnAdd(string id)
    {
        if (!AddedLabTestPriceDetails.ContainsKey(id))
        {
            AddedLabTestPriceDetails.Add(id,LabTestPriceDetails?.FirstOrDefault(x=>x.Id==id));
            TotalPrice += LabTestPriceDetails?.FirstOrDefault(x => x.Id == id)?.Price;
            StateHasChanged();
        }
    }

    protected async Task OnSubmit()
    {
        
        var models = new List<SelectedTestDetailModel>();
        var transactionNumber = DateTime.Now.Ticks.ToString();
        foreach (var (__, test) in AddedLabTestPriceDetails)
        {
            var model = new SelectedTestDetailModel
            {
                Price = test.Price,
                LabTestId = test.Id
                ,TransactionNumber =transactionNumber,
                CreatedBy =Data.PatientId
            };
            models.Add(model);
            AddedLabTestPriceDetails.Remove(test.Id);
        }
        var result= await SelectedTestDetailService.Create(models,TestStatus.Draft,Data.Latitude , Data.Longitude);
        if (result != "")
        {
            TotalPrice = 0;
            await  DeleteALl(PatientFileDetails);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            DialogService.Close();
        }
    }
    protected async Task DeleteALl(IList<PatientFileDetail> details)
    {
        try
        {
            var confirm
                =  await DialogService.Confirm("Are you sure to delete?", "Delete",
                    new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});
            if (confirm.Equals(true))
            {
                var baseAddress = _apiSetting.BaseUrl;
                var result = await PatientFileService.DeleteByPatientId(Data?.PatientId);
                foreach (var file in details)
                {
                    var deleteBuild = new StringBuilder();
                    deleteBuild.Append(baseAddress).Append("/FileSave/TemporaryPatientFiles").Append('/')
                        .Append(file.StoredFileName);
                    await _httpClient.DeleteAsync(deleteBuild.ToString());
                }
                
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);

        }

    }
}