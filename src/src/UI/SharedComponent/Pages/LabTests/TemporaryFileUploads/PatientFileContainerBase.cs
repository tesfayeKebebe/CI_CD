using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Models.PatientFiles;
using SharedComponent.Server.Models.SelectedTestDetails;

namespace SharedComponent.Pages.LabTests.TemporaryFileUploads;
public class PatientFileContainerBase: ComponentBase
{
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    [Inject] private  IPatientFileService  PatientFileService { get; set; }= null!;
    [Inject] private NotificationService NotificationService { get; set; }= null!;
    [Inject] private  HttpClient HttpClient { get; set; }= null!;
    protected List<PatientFileDetail>? Data { get; set; }
    [Inject] private DialogService DialogService { get; set; } = null!;
    private HubConnection? _hubConnection;
    protected bool IsSpinner = true;
    protected override async Task  OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        Data = await GetData();
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{ApiPath.BaseUrl}/PatientFileHub")
            .Build();
        _hubConnection.On<List<PatientFileDetail>>("BroadCastPatientData", (message) =>
        {
            Data.AddRange(message);
            InvokeAsync(StateHasChanged);
        });
        await _hubConnection.StartAsync();
        IsSpinner=false;
    }

    protected async Task Show(IList<PatientFileDetail> details)
    {
        await DialogService.OpenAsync<PatientFileDisplay>("",
            new Dictionary<string, object>() { {"Data",details} },  
            new DialogOptions { Width = "95%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px;"});
    }
    private async Task<List<PatientFileDetail>> GetData()
    {
        return  await PatientFileService.GetPatientFiles();
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
                IsSpinner=true;
                var result = await PatientFileService.Delete(file.Id);
                var deleteBuild = new StringBuilder();
                deleteBuild.Append(ApiPath.BaseUrl).Append("/api/FileSave/TemporaryPatientFiles").Append('/')
                    .Append(file.StoredFileName);
                await HttpClient.DeleteAsync(deleteBuild.ToString());
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                Data = await GetData();
                IsSpinner=false;
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
                    var deleteBuild = new StringBuilder();
                    deleteBuild.Append(ApiPath.BaseUrl).Append("/api/FileSave/TemporaryPatientFiles").Append('/')
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

    protected async Task Add(PatientFileDetail file)
    {
        var patientData = Data.Where(x => x.PatientId == file.PatientId).ToList();
        await DialogService.OpenAsync<AddingPatient>("",
            new Dictionary<string, object>() { {"PatientFileDetails",patientData} },  
            new DialogOptions
            {
                Width = "90%", Height = "100%", Draggable = true, 
                Resizable = true,Style = "position: absolute; Top:80px; "
            });
      Data= await GetData();
      StateHasChanged();
    }
}