using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Health.Mobile.Pdf;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.PatientFiles;

namespace Health.Mobile.Pages.LabTests.TemporaryFileUploads;

public class PatientFileUploadBase: ComponentBase
{
    private List<FileClass> uploadResults = new();
    private int maxAllowedFiles = 10;
    private bool shouldRender;
    protected string pdfName = "";
    protected List<FileClass>? files { get; set; }
    [Inject] private  HttpClient HttpClient { get; set; } = null!;
    [Inject] private  ApiSetting ApiSetting{ get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    [Inject] private IPatientFileService PatientFileService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    protected IList<PatientFileDetail>? Data { get; set; } = null!;
    [Inject] private  IJSRuntime _JsRuntime { get; set; } = null!;
    [Inject] private LocationInformationService LocationInformationService { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        Data = await GetData();
        shouldRender = true;
        StateHasChanged();
    }
    protected  async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var location = await LocationInformationService.CheckAndRequestLocationPermission();

        if (location != null)
        {
            shouldRender = false;
        long maxFileSize = long.MaxValue;
        var upload = false;
       using var content = new MultipartFormDataContent();
       foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
        {
            if (uploadResults.SingleOrDefault(
                    f => f.FileName == file.Name) is null)
            {
                try
                {
                   // files.Add(new() { FileName = file.Name });

                    var fileContent = 
                        new StreamContent(file.OpenReadStream(maxFileSize));

                    fileContent.Headers.ContentType = 
                        new MediaTypeHeaderValue(file.ContentType);
                    
                    content.Add(
                        content: fileContent,
                        name: "\"files\"",
                        fileName: file.Name);

                    upload = true;
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "", ex.Message, 10000);

                    uploadResults.Add(
                        new()
                        {
                            FileName = file.Name, 
                            // ErrorCode = 6, 
                            Uploaded = false
                        });
                }
            }
        }

        if (upload)
        {
            var baseAddress = ApiSetting.BaseUrl;
            var build = new StringBuilder();
            build.Append(baseAddress).Append("/FileSave/patientFiles");
            var postedResponse = await HttpClient.PostAsync(build.ToString(), content);
            files =  await postedResponse.Content
                .ReadFromJsonAsync<List<FileClass>>();
            if (files is not null)
            {
                var models = new List<PatientFileModel>();
                try
                {
                    if (files != null)
                    {
                        models.AddRange(files.Select(file => new PatientFileModel
                        {
                            ContentType = file.ContentType, 
                            FileName = file.FileName, 
                            StoredFileName = file.StoredFileName,                   
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        }));
                    }
                    var result=  await PatientFileService.Create(models);
                    NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                 Data=  await GetData();
                 StateHasChanged();
                }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "", ex.Message, 6000);
                }
                shouldRender = true;
                    StateHasChanged();
            }
        }
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, "", "Please reload your system. your location is null", 20000);
        }
        
    }
        protected async Task Show(IList<PatientFileDetail> details)
    {
        await DialogService.OpenAsync<PatientFileDisplay>("",
            new Dictionary<string, object>() { {"Data",details} },  
            new DialogOptions { Width = "95%", Height = "100%", Style = "position: absolute; Top:80px; " });
    }
    private async Task<IList<PatientFileDetail>> GetData()
    {
        return  await PatientFileService.GetPatientFilesByPatientId();
    }
    protected async Task Delete(PatientFileDetail file)
    {
        try
        {
            var confirm
                =  await DialogService.Confirm("Are you sure to delete?", "Delete",
                    new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});
            if (confirm.Equals(true))
            {
                var baseAddress = ApiSetting.BaseUrl;
                var result = await PatientFileService.Delete(file.Id);
                var deleteBuild = new StringBuilder();
                deleteBuild.Append(baseAddress).Append("/FileSave/TemporaryPatientFiles").Append('/')
                    .Append(file.StoredFileName);
                await HttpClient.DeleteAsync(deleteBuild.ToString());
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                Data = await GetData();
                StateHasChanged();
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);

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
                var baseAddress = ApiSetting.BaseUrl;
                var result = await PatientFileService.DeleteByPatientId(details.FirstOrDefault()?.PatientId);
                foreach (var file in details)
                {
                    var deleteBuild = new StringBuilder();
                    deleteBuild.Append(baseAddress).Append("/FileSave/TemporaryPatientFiles").Append('/')
                        .Append(file.StoredFileName);
                    await HttpClient.DeleteAsync(deleteBuild.ToString());
                }

                Data = await GetData();
                StateHasChanged();
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);

        }

    }

}