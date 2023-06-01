using System.Collections.ObjectModel;
using System.Text;
using Health.Mobile.Pages.ViewModel;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Enums;
using Health.Mobile.Server.Models.SelectedTestDetails;
using Health.Mobile.StateManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;

namespace Health.Mobile.Pages.LabTests.Drafts;

public class DraftBase: ComponentBase
{
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private NotificationStateService NotificationStateService { get; set; }= null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private  ApiSetting ApiSetting { get; set; }= null!;
    protected List<SelectedTestDetail> SelectedTestDetails { get; set; } = new List<SelectedTestDetail>();
    private HubConnection? _hubConnection;
    protected override async Task OnInitializedAsync()
    {
        await GetData();
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(ApiSetting.BaseUrl.Replace("/api", "/DraftHub"))
            .Build();
        _hubConnection.On<SelectedTestDetail>("BroadCastDraftData", (message) =>
        {
            if (SelectedTestDetails.FirstOrDefault(x => x.TransactionNumber == message.TransactionNumber) != null)
            {
                return;
            }
            SelectedTestDetails.Add(message);
           InvokeAsync(StateHasChanged);
        });
        await _hubConnection.StartAsync();
    }
    protected async Task OnDone(SelectedTestDetail selected)
    {
        var model = new SelectedLabTestDetailViewModel
        {
            Name = selected.PatientName,
            TransactionNumber = selected.TransactionNumber
        };
        await DialogService.OpenAsync<ApproveDraft>("",
            new Dictionary<string, object>() { {"SelectedTestDetail",model} },  
            new DialogOptions { Width = "90%", Height = "100%", Style = "position: absolute; Top:80px; " });
        await GetData();
        StateHasChanged();
    }
    protected async Task OnDelete(string transactionNumber)
    {
        try
        {
            var confirm
                =  await DialogService.Confirm("Are you sure?", "Delete",
                    new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});
            if (confirm.Equals(true))
            {
                var result = await SelectedTestDetailService.DeleteSelectedDetailByTransaction(transactionNumber);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                await GetData();
                StateHasChanged();
            }
            

        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
    


    private async Task GetData()
    {
        SelectedTestDetails = await SelectedTestDetailService.GetSelectedTestDetails(TestStatus.Draft);
    }
    
}