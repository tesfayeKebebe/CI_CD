using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Radzen;
using Health.Mobile.Pdf;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Enums;
using Health.Mobile.Server.Models.LabTestResults;
using Health.Mobile.Server.Models.SelectedTestDetails;
using Health.Mobile.Server.Models.SelectedTestStatuses;

namespace Health.Mobile.Pages.LabTests;

public class TestResultBase : ComponentBase
{
    [Inject] private ITestResultService ResultService { get; set; } = null!;
    [Inject] private  ISelectedTestStatusService SelectedTestStatus { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService{get; set; } = null!;
    [Inject] private  HttpClient _httpClient { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
   [Parameter] public TestResultDetail Model{ get; set; } = new();
   [Inject] private  ApiSetting _apiSetting{ get; set; } = null!;
    protected bool IsEdit = false;
    protected bool HasPermission;
    private bool shouldRender;
    protected override bool ShouldRender() => shouldRender;
    private List<FileClass> uploadResults = new();
    private Lazy<IJSObjectReference> NewTabModule = new();
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    protected string pdfUrl = "";
    protected FileClass? files { get; set; }
    protected FileClass? Originalfile { get; set; }
    private string baseAddress ="";
    protected override async Task OnInitializedAsync()
    {
        baseAddress = _apiSetting.BaseUrl;
        var build = new StringBuilder();
        if (Model.StoredFileName != null)
        {
            files = new()
            {
                StoredFileName = Model.StoredFileName,
                ContentType = Model.ContentType,
                FileName = Model.FileName
            };
            Originalfile = files;
            pdfUrl=   build.Append(baseAddress).Append("/FileSave/pdf/").Append(Model.StoredFileName).ToString();    
        }
        var   SessionStore=   await SessionStoreService.Get();
        if (SessionStore.Authentication?.Roles != null && SessionStore.Authentication != null && (SessionStore.Authentication.Roles.Contains("administrator")|| SessionStore.Authentication.Roles.Contains("supervisor")) )
        {
            HasPermission = true;
        }
        if (Model.Id != null)
        {
            IsEdit = true;
        }
        await InvokeAsync(StateHasChanged);
    }
    

    protected  async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        shouldRender = false;
        // var file = e.File;
        // if (file != null)
        // {
        //     var buffers = new byte[file.Size];
        //     await file.OpenReadStream(file.Size).ReadAsync(buffers);
        //     FileUpload.ContentType = file.ContentType;
        //     FileUpload.FileName = file.Name;
        //     FileUpload.Attachments = buffers;
        // }
       var upload = false;
       long maxFileSize = long.MaxValue;
       using var content = new MultipartFormDataContent();
       foreach (var file in e.GetMultipleFiles(1))
        {
            if (uploadResults.SingleOrDefault(
                    f => f.FileName == file.Name) is null)
            {
                try
                {
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
                catch (Exception)
                {
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
            var build = new StringBuilder();
            build.Append(baseAddress).Append("/FileSave");
            var postedResponse = await _httpClient.PostAsync(build.ToString(), content);
            files = await postedResponse.Content
                .ReadFromJsonAsync<FileClass>();
            if (files is not null)
            {
                if (Originalfile?.StoredFileName != null)
                {
                    var deleteBuild = new StringBuilder();
                    deleteBuild.Append(baseAddress).Append("/FileSave").Append('/').Append(Originalfile.StoredFileName);
                     await _httpClient.DeleteAsync(deleteBuild.ToString());
                     await ResetFile();
                }
                Originalfile = files;
                var pdfBaseAddress = _apiSetting.BaseUrl;
                var pdfBuild = new StringBuilder();
                pdfUrl=   pdfBuild.Append(pdfBaseAddress).Append("/FileSave/pdf/").Append(files.StoredFileName).ToString();
                shouldRender = true;
                StateHasChanged();
            }
        }
        
    }

    private async Task ResetFile()
    {
        if (Model.StoredFileName != null)
        {
            Model.StoredFileName = null;
            Model.FileName = null;
            Model.ContentType = null;
        }
    }
    protected async Task DeleteUploadedFile()
    {
        if (files != null)
        {
            var deleteBuild = new StringBuilder();
            deleteBuild.Append(baseAddress).Append("/FileSave").Append('/').Append(files?.StoredFileName?? Model.StoredFileName);
            await _httpClient.DeleteAsync(deleteBuild.ToString());
            pdfUrl = "";
            await ResetFile();
        }
    }
     private async Task GetData()
     {
         Model=    await ResultService.Get(Model.TransactionNumber);
         if (Model.Id != null)
         {
             IsEdit = true;
             if (Model.StoredFileName!=null)
             {
                 Originalfile = new FileClass
                 {
                     ContentType = Model.ContentType,
                     FileName = Model.FileName,
                     StoredFileName = Model.StoredFileName
                 };
             }   
         }

     }

     protected async Task GoToNewTab()
     {
         NewTabModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
         var build = new StringBuilder();
         build.Append(baseAddress).Append("/FileSave/pdf/");
         await NewTabModule.Value.InvokeVoidAsync("load",$"{build.ToString()}{Model?.StoredFileName}");
     }
     protected void OnBack()
    {
        DialogService.Close();
    }

    protected async Task OnSave()
    {
        if (Model is {Id: null} && !IsEdit)
        {
            try
            {
                var testResult = new Health.Mobile.Server.Models.LabTestResults.TestResult
                {
                    Description = Model.Description,
                    TransactionNumber = Model.TransactionNumber,
                    PatientId = Model.PatientId,
                    FileName = files?.FileName,
                    ContentType = files?.ContentType,
                    StoredFileName = files?.StoredFileName
                };
                var result = await ResultService.Create(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                StateHasChanged();
            }
            catch (Exception e)
            {
                NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }

        }
        else
        {
            try
            {
                var testResult = new TestResultDetail
                {
                    Description = Model?.Description,
                    Id = Model!.Id,
                    FileName = files?.FileName,
                    ContentType = files?.ContentType,
                    StoredFileName = files?.StoredFileName
                };
                var result = await ResultService.Update(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                StateHasChanged();
            }
            catch (Exception e)
            {
                NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
            }
        }
    }

    protected async Task OnFinish()
    {
        try
        {
            if (HasPermission )
            {
                if (Model is {IsCompleted: true})
                {
                    var testResult = new SelectedTestStatus
                    {
                        TransactionNumber = Model.TransactionNumber,
                        TestStatus = TestStatus.Completed,
                    };
                    var result=     await SelectedTestStatus.Update(testResult);
                    NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                    DialogService.Close();
                    
                }
                else
                {
                    if (Model != null)
                    {
                        var approval = new LabTestResultApproval
                        {
                            IsCompleted = Model.IsCompleted,
                            Id = Model.Id,
                            Reason = Model.Reason,
                        };
                        var result=    await ResultService.Approval(approval);
                        NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                        DialogService.Close();
                    }
                }
            }
            else
            {
                var testResult = new TestResultDetail
                {
                    Description = Model?.Description,
                    Id = Model!.Id,
                    IsCompleted = true,
                    FileName = files?.FileName,
                    ContentType = files?.ContentType,
                    StoredFileName = files?.StoredFileName
                };
                var result = await ResultService.Update(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                DialogService.Close();
            }

        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}