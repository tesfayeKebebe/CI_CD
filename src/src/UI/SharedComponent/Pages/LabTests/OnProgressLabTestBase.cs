using System.Collections.ObjectModel;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Enums;
using SharedComponent.Server.Models.LabTestResults;
using SharedComponent.Server.Models.SelectedTestDetails;
using SharedComponent.Server.Models.SelectedTestStatuses;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.LabTests;

public class OnProgressLabTestBase : ComponentBase
{
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private SessionStoreService SessionStoreService { get; set; }= null!;
    [Inject] private NavigationManager NavigationManager { get; set; }= null!;
    [Inject] private ITestResultService ResultService { get; set; } = null!;
    [Inject] private NotificationStateService NotificationStateService { get; set; }= null!;
    private List<SelectedTestDetail> Originaldata { get; set; } = new List<SelectedTestDetail>();
    protected TestResultDetail Model { get; set; } = new();
    protected string? Search { get; set; }
    protected bool ClearEnabled { get; set; }
    protected bool IsSpinner = true;
    protected List<SelectedTestDetail> SelectedTestDetails { get; set; } = new List<SelectedTestDetail>();
    private HubConnection? _hubConnection;
    private bool _isAll;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        var session=   await SessionStoreService.Get();
        if (session.Authentication?.Roles != null && session.Authentication != null && (session.Authentication.Roles.Contains("administrator")|| session.Authentication.Roles.Contains("LaboratoryTechnologist")|| session.Authentication.Roles.Contains("supervisor")) )
        {
            _isAll = true;
        }
        await GetData();
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{ApiPath.BaseUrl}/SelectedLabTestHub")
            .Build();
        _hubConnection.On<SelectedTestDetail>("BroadCastData", (message) =>
        {
            if(_isAll)
            {
                if (Originaldata.FirstOrDefault(x => x.TransactionNumber == message.TransactionNumber) != null)
                {
                    return;
                }
                Originaldata.Add(message);
                SelectedTestDetails = Originaldata;
                InvokeAsync(StateHasChanged);
            }
            else
            {
                var userId=    Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!));
                if (userId == message.AssignUserId)
                {
                    Originaldata.Add(message);  
                    SelectedTestDetails = Originaldata;
                    InvokeAsync( StateHasChanged);
                }
                else
                {
                    var firstData = Originaldata.FirstOrDefault(x => x.TransactionNumber == message.TransactionNumber);
                    if (firstData == null)
                    {
                        return;
                    }
                    Originaldata.Remove(firstData);
                    SelectedTestDetails = Originaldata;
                    InvokeAsync( StateHasChanged);
                }
            }
            
        });
        await _hubConnection.StartAsync();
    }

    protected async Task OnDone(SelectedTestDetail selected)
    {
        var data = await GetTestResult(selected.TransactionNumber);
        data.TransactionNumber = selected.TransactionNumber;
        data.PatientName = selected.PatientName;
        data.PatientId = selected.PatientId;
        // NavigationManager.NavigateTo("/test-result");
        await DialogService.OpenAsync<TestResult>("Test Result",
            new Dictionary<string, object>() { {"Model",data} },  
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; " });
       await GetData();
       StateHasChanged();
    }
    protected async Task OnDetail(SelectedTestDetail selected)
    {
        var model = new SelectedLabTestDetailViewModel
        {
            Name = selected.PatientName,
            TransactionNumber = selected.TransactionNumber
        };
        await DialogService.OpenAsync<SelectedLabTestDetail>("Detail",
            new Dictionary<string, object>() { {"SelectedLab",model} },  
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; " });
        await GetData();
        StateHasChanged();
    }
    protected async Task OnApprove(SelectedTestDetail selected)
    {
        var data = await GetTestResult(selected.TransactionNumber);
        data.TransactionNumber = selected.TransactionNumber;
        data.PatientName = selected.PatientName;
        data.PatientId = selected.PatientId;
        await DialogService.OpenAsync<TestResult>("Test Result",
            new Dictionary<string, object>() { {"Model",data} },  
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; " });
        await GetData();
        StateHasChanged();
    }

    private async Task<TestResultDetail> GetTestResult(string transactionNumber)
    {
      return  await ResultService.Get(transactionNumber);
    }
    protected async Task OnAssign(SelectedTestDetail selected)
    {
        var assign = new UpdateAssignedUser
        {
            TransactionNumber = selected.TransactionNumber,
            AssignedUser = selected.AssignUserId
        };
        await DialogService.OpenAsync<EditAssignUser>("Assign User",
            new Dictionary<string, object>() { {"Model",assign} },  
            new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; " });
        await GetData();
        StateHasChanged();
    }
    
    

    protected async Task OnMapVisit(SelectedTestDetail selected)
    {
        var session = await SessionStoreService.Get();
        var location = new PatientLocation
        {
            Latitude = selected.Latitude,
            Longitude = selected.Longitude,
            TransactionNumber = selected.TransactionNumber,
            SampleNumber = selected.SampleNumber,
            PatientName = selected.PatientName,
            IsSampleTaken = selected.IsSampleTaken
        };
        session.Location = location;
       await SessionStoreService.Set(session);
       NavigationManager.NavigateTo("/maps");
       
    }

    private async Task GetData()
    {
        IsSpinner=true;
       if(_isAll){
           Originaldata = await SelectedTestDetailService.GetSelectedTestDetails(TestStatus.OnProgress);
           SelectedTestDetails = Originaldata;
            IsSpinner=false;
       }
       else
       {
           Originaldata = await SelectedTestDetailService.GetSelectedTestDetailsByUser();
           SelectedTestDetails = Originaldata;
            IsSpinner=false;
        }

    }
    protected void Clear()
    {
        Search = null;
        ClearEnabled = false;
        SelectedTestDetails = Originaldata.ToList();
    }
    protected void OnInputChanged(ChangeEventArgs obj)
    {
        ClearEnabled = !string.IsNullOrEmpty(obj.Value?.ToString());
        Search = obj.Value?.ToString();
        SearchAsync();
        StateHasChanged();
    }
    private void SearchAsync()
    {
        if(Search==null) return;

        var data = Originaldata.Where(x =>
            Search != null && x.PatientName.ToLower().Contains(Search.ToLower()) ||
            (x.AssignedUser != null && x.AssignedUser.ToLower().Contains(Search.ToLower()))
            ||
           ( int.TryParse(Search, out var n) && x.SampleNumber == Convert.ToInt32(n))).ToList();
        if (data.Count <=0)
        {
            return;
        }

        SelectedTestDetails = new List<SelectedTestDetail>();
        foreach (var item in  data)
        {
            SelectedTestDetails.Add(item);
        }

    }
}