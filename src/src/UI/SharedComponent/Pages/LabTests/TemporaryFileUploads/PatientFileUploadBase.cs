using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Models;
using SharedComponent.Server.Models.PatientFiles;
namespace SharedComponent.Pages.LabTests.TemporaryFileUploads;

public class PatientFileUploadBase: ComponentBase
{
    private List<FileClass> uploadResults = new();
    private bool _shouldRender;
    protected string PdfName = "";
    protected List<FileClass>? files { get; set; }
    protected List<FileClass> uploadFiles { get; set; } = new();
    [Inject] private  IFileSaverService FileSaverService { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    [Inject] private IPatientFileService PatientFileService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    protected IList<PatientFileDetail>? Data { get; set; }
    private WindowNavigatorGeolocation _geolocation= null!;
    private GeolocationPosition _geolocationPosition= null!;
    [Inject] private  IJSRuntime JsRuntime { get; set; }= null!;
    [Inject] private SessionStoreService SessionStoreService { get; set; } = null!;
    protected bool isDisabled = false;
    protected bool IsSpinner = true;
    protected string Text { get; set; } = "Upload Files";
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        var window = await JsRuntime.Window();
        var navigator = await window.Navigator();
        _geolocation = navigator.Geolocation;
        _geolocationPosition = (await _geolocation.GetCurrentPosition(
            new PositionOptions
            {
                EnableHighAccuracy = true,
                MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                TimeoutTimeSpan = TimeSpan.FromMinutes(5)
            })).Location;
        Data = await GetData();
        _shouldRender = true;
        IsSpinner=false;
        StateHasChanged();
    }
    protected  async Task HandleFileSelected(InputFileChangeEventArgs e)
    {

        if (_geolocationPosition.Coords != null)
        {
            IsSpinner=true;
            _shouldRender = false;
        long maxFileSize = 1024*1024*20;
            var maxAllowedFiles = 10;
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
            var files = await FileSaverService.PostFiles(content);
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
                            Latitude = _geolocationPosition.Coords.Latitude,
                            Longitude = _geolocationPosition.Coords.Longitude
                        }));
                            var result = await PatientFileService.Create(models);
                            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                            Data = await GetData();
                            StateHasChanged();
                        }

                    }
                catch (Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "", ex.Message, 6000);
                }

            }
        }
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, "", "Please reload your system. your location is null", 20000);
        }
        IsSpinner=false;
        _shouldRender = true;

    }
   protected async Task OnChange(InputFileChangeEventArgs e)
    {

        if(e.FileCount>0)
        {
            IsSpinner=true;
            var files = e.GetMultipleFiles();
            // get the files selected by the users
            var session = await SessionStoreService.Get();
            foreach (var file in files)
            {

               // var resizedFile = await file.RequestImageFileAsync(file.ContentType, 640, 480); // resize the image file
                                                                                                // allocate a buffer to fill with the file's data
                using (var stream = new System.IO.MemoryStream())
                {
                    await file.OpenReadStream(1024*1024*20).CopyToAsync(stream);
                    var buf = stream.ToArray();
                    uploadFiles.Add(new FileClass
                    {
                        Base64data = Convert.ToBase64String(buf),
                        ContentType = file.ContentType,
                        FileName = file.Name,
                        Latitude= _geolocationPosition.Coords.Latitude,
                        Longitude = _geolocationPosition.Coords.Longitude,
                        CreatedBy= Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
                    });
                    // convert to a base64 string!!
                }
            }
            Text="Click upload to continue";
            IsSpinner=false;
            StateHasChanged();
        }

    }
   protected async Task Upload()
    {
            try
            {
                if (uploadFiles.Count>0)
                {
                var session = SessionStoreService.Get();
                IsSpinner=true;
                isDisabled = true;
                var result=  await FileSaverService.PostFiles(uploadFiles);
                    //var models = new List<FileClass>();
                    //models.AddRange(uploadFiles.Select(file => new FileClass
                    //{
                    //    ContentType = file.ContentType,
                    //    FileName = file.FileName,
                    //    StoredFileName = file.StoredFileName,
                    //    Base64data= file.Base64data,
                    //    Latitude = _geolocationPosition.Coords.Latitude,
                    //    Longitude = _geolocationPosition.Coords.Longitude,
                    //   CreatedBy= Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!))
                    //}));
                    //var result = await PatientFileService.Create(models);
                    NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                    Data = await GetData();
                    isDisabled = false;
                    IsSpinner=false;
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "", ex.Message, 6000);
            }
        
    }

    protected async Task Show(IList<PatientFileDetail> details)
    {
        await DialogService.OpenAsync<PatientFileDisplay>("",
            new Dictionary<string, object>() { {"Data",details} },  
            new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; " });
    }
    private async Task<IList<PatientFileDetail>> GetData()
    {
        return await PatientFileService.GetPatientFilesByPatientId(); ;
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
                var result = await PatientFileService.Delete(file.Id);
                await FileSaverService.Delete(file.StoredFileName);
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
                var result = await PatientFileService.DeleteByPatientId(details.FirstOrDefault()?.PatientId);
                foreach (var file in details)
                {
                    await FileSaverService.Delete(file.StoredFileName);
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