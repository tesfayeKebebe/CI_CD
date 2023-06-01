using System.Text;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Api.Impl;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.Server.Models.PatientFiles;
using SharedComponent.Server.Models.SelectedTestDetails;

namespace SharedComponent.Pages.LabTests.TemporaryFileUploads;

public class AddingPatientBase: ComponentBase
{
    [Inject] private ILabTestService LabTestService { get; set; } = null!;
    [Inject] private ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService { get; set; }= null!;
    [Inject] private  HttpClient HttpClient { get; set; }= null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    private SessionStore _sessionStore = new();
    protected PatientFileDetail? Data = new();
    [Parameter] public  List<PatientFileDetail> PatientFileDetails { get; set; }= null!;
    [Inject] private  IPatientFileService  PatientFileService { get; set; }= null!;
    protected SelectedTestDetailModel Model { get; set; } = new ();
    protected double? TotalPrice = 0;
    protected List<LabTestPriceDetail> LabTestPriceDetails { get; set; } = new List<LabTestPriceDetail>();
    protected readonly Dictionary<string, LabTestPriceDetail?> AddedLabTestPriceDetails = new Dictionary<string, LabTestPriceDetail?>();
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
       
        Data=  PatientFileDetails.FirstOrDefault();
        LabTestPriceDetails=  await LabTestService.GetLabTestsDropDown();
        _sessionStore = await SessionStoreService.Get();
        IsSpinner=false;
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
        IsSpinner=true;
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
            IsSpinner=false;
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
                var result = await PatientFileService.DeleteByPatientId(Data?.PatientId);
                foreach (var file in details)
                {
                    var deleteBuild = new StringBuilder();
                    deleteBuild.Append(ApiPath.BaseUrl).Append("/api/FileSave/TemporaryPatientFiles").Append('/')
                        .Append(file.StoredFileName);
                    await HttpClient.DeleteAsync(deleteBuild.ToString());
                }
                
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);

        }

    }
}