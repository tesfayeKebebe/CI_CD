using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Models;
using SharedComponent.Server.Models.LabTestResults;
using SharedComponent.Server.Models.SelectedTestDetails;
using SharedComponent.Server.Models.SelectedTestStatuses;

namespace SharedComponent.Pages.LabTests;

public class TestResultBase : ComponentBase
{
    [Inject] private ITestResultService ResultService { get; set; } = null!;
    [Inject] private  ISelectedTestStatusService SelectedTestStatus { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; } = null!;
    [Inject] private  SessionStoreService SessionStoreService{get; set; }= null!;
    [Inject] private  IFileSaverService FileSaverService { get; set; }= null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
   [Parameter] public TestResultDetail Model{ get; set; } = new();
   protected bool HasPermission;
    private bool _shouldRender;
    protected override bool ShouldRender() => _shouldRender;
    private readonly List<FileClass> _uploadResults = new();
    private Lazy<IJSObjectReference> _newTabModule = new();
    [Inject] private IJSRuntime JsRuntime { get; set; }= null!;
    protected string PdfUrl = "";
    protected FileClass? Files { get; set; }
    protected FileClass? Originalfile { get; set; }
    protected string inputeMessage = "Choose File";
    protected bool isDisabled = false;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        var build = new StringBuilder();
        if (Model.StoredFileName != null)
        {
            Files = new()
            {
                StoredFileName = Model.StoredFileName,
                ContentType = Model.ContentType,
                FileName = Model.FileName
            };
            Originalfile = Files;
            PdfUrl=   build.Append(ApiPath.BaseUrl).Append("/api/FileSave/pdf/").Append(Model.StoredFileName).ToString();    
        }
        var   SessionStore=   await SessionStoreService.Get();
        if (SessionStore.Authentication?.Roles != null && SessionStore.Authentication != null && (SessionStore.Authentication.Roles.Contains("administrator")|| SessionStore.Authentication.Roles.Contains("supervisor")) )
        {
            HasPermission = true;
        }
        IsSpinner=false;
        StateHasChanged();

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            _shouldRender =true;
        }
    }
    protected  async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        _shouldRender = false;
        // var file = e.File;
        // if (file != null)
        // {
        //     var buffers = new byte[file.Size];
        //     await file.OpenReadStream(file.Size).ReadAsync(buffers);
        //     FileUpload.ContentType = file.ContentType;
        //     FileUpload.FileName = file.Name;
        //     FileUpload.Attachments = buffers;
        // }
        isDisabled=true;
       var upload = false;
       long maxFileSize = long.MaxValue;
        IsSpinner=true;
       using var content = new MultipartFormDataContent();
       foreach (var file in e.GetMultipleFiles(1))
        {
            if (_uploadResults.SingleOrDefault(
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
                catch (Exception ex)
                {
                    _uploadResults.Add(
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
            Files = await FileSaverService.PostResultFiles(content);
            if (Files is not null)
            {
                if (Originalfile?.StoredFileName != null)
                {
                     await FileSaverService.DeleteResultFile(Originalfile.StoredFileName);
                     await ResetFile();
                }
                Originalfile = Files;
                var pdfBuild = new StringBuilder();
                PdfUrl=   pdfBuild.Append(ApiPath.BaseUrl).Append("/api/FileSave/pdf/").Append(Files.StoredFileName).ToString();
                _shouldRender = true;
                isDisabled= false;
                StateHasChanged();

            }
        }
        IsSpinner=false;
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
        if (Files != null)
        {
            await FileSaverService.DeleteResultFile(Files?.StoredFileName?? Model.StoredFileName);
            PdfUrl = "";
            await ResetFile();
        }
    }
     private async Task GetData()
     {
         Model=    await ResultService.Get(Model.TransactionNumber);
         if (Model is {Id: { }, StoredFileName: { }})
         {
             Originalfile = new FileClass
             {
                 ContentType = Model.ContentType,
                 FileName = Model.FileName,
                 StoredFileName = Model.StoredFileName
             };
         }

     }

     protected async Task GoToNewTab()
     {
         _newTabModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
         var build = new StringBuilder();
         build.Append(ApiPath.BaseUrl).Append("/api/FileSave/pdf/");
         await _newTabModule.Value.InvokeVoidAsync("load",$"{build.ToString()}{Model?.StoredFileName}");
     }
     protected void OnBack()
    {
        DialogService.Close();
    }

    protected async Task OnSave()
    {
        if (Model is {Id: null})
        {
            try
            {
                IsSpinner=true;
                var testResult = new SharedComponent.Server.Models.LabTestResults.TestResult
                {
                    Description = Model.Description,
                    TransactionNumber = Model.TransactionNumber,
                    PatientId = Model.PatientId,
                    FileName = Files?.FileName,
                    ContentType = Files?.ContentType,
                    StoredFileName = Files?.StoredFileName
                };
                var result = await ResultService.Create(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                IsSpinner=false;
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
                IsSpinner=true;
                var testResult = new TestResultDetail
                {
                    Description = Model?.Description,
                    Id = Model!.Id,
                    FileName = Files?.FileName,
                    ContentType = Files?.ContentType,
                    StoredFileName = Files?.StoredFileName
                };
                var result = await ResultService.Update(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                IsSpinner=false;
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
                IsSpinner=true;
                if (Model is {IsCompleted: true})
                {
                    var testResult = new SelectedTestStatus
                    {
                        TransactionNumber = Model.TransactionNumber,
                        TestStatus = TestStatus.Completed,
                    };
                    var result=     await SelectedTestStatus.Update(testResult);
                    NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                    IsSpinner=false;
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
                IsSpinner=true;
                var testResult = new TestResultDetail
                {
                    Description = Model?.Description,
                    Id = Model!.Id,
                    IsCompleted = true,
                    FileName = Files?.FileName,
                    ContentType = Files?.ContentType,
                    StoredFileName = Files?.StoredFileName
                };
                var result = await ResultService.Update(testResult);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                IsSpinner=false;
                DialogService.Close();
            }

        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}