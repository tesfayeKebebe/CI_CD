using System.Collections.ObjectModel;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Models.SelectedTestDetails;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.LabTests.Drafts;

public class DraftBase: ComponentBase
{
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private NotificationStateService NotificationStateService { get; set; }= null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    protected List<SelectedTestDetail> SelectedTestDetails { get; set; } = new List<SelectedTestDetail>();
    private HubConnection? _hubConnection;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        await GetData();
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{ApiPath.BaseUrl}/DraftHub")
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
        IsSpinner=true;
        SelectedTestDetails = await SelectedTestDetailService.GetSelectedTestDetails(TestStatus.Draft);
        IsSpinner=false;

    }
    
}