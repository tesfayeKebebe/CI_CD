using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.PatientFiles;
using Health.Mobile.Server.Models.SelectedTestDetails;

namespace Health.Mobile.Pages.LabTests.TemporaryFileUploads;
public class PatientFileContainerBase: ComponentBase
{
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    [Inject] private  IPatientFileService  PatientFileService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private  HttpClient _httpClient { get; set; } = null!;
    [Inject] private  ApiSetting _apiSetting{ get; set; } = null!;
    protected List<PatientFileDetail>? Data { get; set; }
    [Inject] private DialogService DialogService { get; set; } = null!;
    private HubConnection? hubConnection;
    protected override async Task  OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        Data = await GetData();
        hubConnection = new HubConnectionBuilder()
            .WithUrl(_apiSetting.BaseUrl.Replace("/api", "/PatientFileHub"))
            .Build();
        hubConnection.On<List<PatientFileDetail>>("BroadCastPatientData", (message) =>
        {
            Data.AddRange(message);
           InvokeAsync(StateHasChanged);
        });
        await hubConnection.StartAsync();
    }

    protected async Task Show(IList<PatientFileDetail> details)
    {
        await DialogService.OpenAsync<PatientFileDisplay>("",
            new Dictionary<string, object>() { {"Data",details} },  
            new DialogOptions { Width = "95%", Height = "100%", Draggable = true, Resizable = true, Style = "position: absolute; Top:80px; " });
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
                var baseAddress = _apiSetting.BaseUrl;
                var result = await PatientFileService.Delete(file.Id);
                var deleteBuild = new StringBuilder();
                deleteBuild.Append(baseAddress).Append("/FileSave/TemporaryPatientFiles").Append('/')
                    .Append(file.StoredFileName);
                await _httpClient.DeleteAsync(deleteBuild.ToString());
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
                var baseAddress = _apiSetting.BaseUrl;
                var result = await PatientFileService.DeleteByPatientId(details.FirstOrDefault()?.PatientId);
                foreach (var file in details)
                {
                    var deleteBuild = new StringBuilder();
                    deleteBuild.Append(baseAddress).Append("/FileSave/TemporaryPatientFiles").Append('/')
                        .Append(file.StoredFileName);
                    await _httpClient.DeleteAsync(deleteBuild.ToString());
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
                Resizable = true,
                Style = "position: absolute; Top:80px; "
            });
      Data= await GetData();
      StateHasChanged();
    }
}