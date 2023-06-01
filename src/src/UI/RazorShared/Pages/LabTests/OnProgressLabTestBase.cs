using System.Collections.ObjectModel;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Constants;
using RazorShared.Server.Enums;
using RazorShared.Server.Models.SelectedTestDetails;
using RazorShared.Server.Models.SelectedTestStatuses;

namespace RazorShared.Pages.LabTests;

public class OnProgressLabTestBase : ComponentBase
{
    [Inject] private  ISelectedTestDetailService SelectedTestDetailService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private SessionStoreService SessionStoreService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private  ApiSetting ApiSetting { get; set; }
    private List<SelectedTestDetail> Originaldata { get; set; } = new List<SelectedTestDetail>();
    protected string? Search { get; set; }
    protected bool ClearEnabled { get; set; }
    protected List<SelectedTestDetail> SelectedTestDetails { get; set; } = new List<SelectedTestDetail>();
    private HubConnection? hubConnection;
    private bool _isAll;
    protected override async Task OnInitializedAsync()
    {
        var session=   await SessionStoreService.Get();
        if (session.Authentication?.Roles != null && session.Authentication != null && (session.Authentication.Roles.Contains("administrator")|| session.Authentication.Roles.Contains("LaboratoryTechnologist")|| session.Authentication.Roles.Contains("supervisor")) )
        {
            _isAll = true;
        }
        await GetData();
        hubConnection = new HubConnectionBuilder()
            .WithUrl(ApiSetting.BaseUrl.Replace("/api", "/SelectedLabTestHub"))
            .Build();

        hubConnection.On<SelectedTestDetail>("BroadCastData", (message) =>
        {
            if(_isAll)
            {
                if (Originaldata.FirstOrDefault(x => x.TransactionNumber == message.TransactionNumber) != null)
                {
                    return;
                }

                Originaldata.Add(message);
                SelectedTestDetails = Originaldata;
                StateHasChanged();
            }
            else
            {
                var userId=    Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!));
                if (userId == message.AssignUserId)
                {
                    Originaldata.Add(message);  
                    SelectedTestDetails = Originaldata;
                    StateHasChanged();
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
                    StateHasChanged();
                }
            }


        });
        await hubConnection.StartAsync();
    }

    protected async Task OnDone(SelectedTestDetail selected)
    {
        await DialogService.OpenAsync<TestResult>("Test Result",
            new Dictionary<string, object>() { {"selectedTestDetail",selected} },  
            new DialogOptions { Width = "50%", Height = "90%" });
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
            new DialogOptions { Width = "50%", Height = "90%"  });
        await GetData();
        StateHasChanged();
    }
    protected async Task OnApprove(SelectedTestDetail selected)
    {
        await DialogService.OpenAsync<TestResult>("Test Result",
            new Dictionary<string, object>() { {"selectedTestDetail",selected} },  
            new DialogOptions { Width = "50%", Height = "90%" });
        await GetData();
        StateHasChanged();
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
            new DialogOptions { Width = "50%", Height = "90%" });
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
       if(_isAll){
           Originaldata = await SelectedTestDetailService.GetSelectedTestDetails(TestStatus.OnProgress);
           SelectedTestDetails = Originaldata;
       }
       else
       {
           Originaldata = await SelectedTestDetailService.GetSelectedTestDetailsByUser();
           SelectedTestDetails = Originaldata;
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