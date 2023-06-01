using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using Health.Mobile.Pages.Securities;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models;
using Health.Mobile.Server.Models.PatientFiles;
using Health.Mobile.Server.Models.SelectedTestDetails;
using Health.Mobile.StateManagement;

namespace Health.Mobile.Shared
{
    public class MainHeaderBase : ComponentBase , IDisposable
    { 
         protected int HeaderTabIndex { get; set; }
         [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
         [Inject]  private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private  NavigationStateService NavigationStateService { get; set; } = null!;
        [Inject] private DialogService DialogService { get; set; } = null!;
        [Inject] private BearerAuthStateProvider BearerAuthStateProvider { get; set; } = null!;
        [Inject] private  SessionStoreService SessionStoreService { get; set; } = null!;
        [Inject] private  IAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NotificationStateService NotificationStateService { get; set; }= null!;
        [Inject] private  ApiSetting ApiSetting { get; set; }= null!;
        protected int PatientFile { get; set; } = 0;
        protected int Draft { get; set; } = 0;
        protected int Progress{ get; set; } = 0;
        protected bool IsAuthenticated { get; set; }
        private PeriodicTimer _timer= new (TimeSpan.FromMinutes(1)) ;
        protected string? UserName { get; set; }
        protected DataCounterDto? DataCounter = new DataCounterDto();
        private HubConnection? _hubConnection;
        private HubConnection? _draftHubConnection;
        private HubConnection? _patientFileHubConnection;
        protected override async Task OnInitializedAsync()
        {
         var user =await BearerAuthStateProvider.GetAuthenticationStateAsync();
            if (user.User.Identity is {IsAuthenticated: true})
            {
                var session = await SessionStoreService.Get();
                UserName = session?.Authentication?.Username;
                IsAuthenticated = true;
                if (session?.Authentication?.Roles == null || !session.Authentication.Roles.Contains("user"))
                {
                    _hubConnection = new HubConnectionBuilder()
                        .WithUrl(ApiSetting.BaseUrl.Replace("/api", "/SelectedLabTestHub"))
                        .Build();
                    _hubConnection.On<SelectedTestDetail>("BroadCastData", (message) =>
                    {
                        if (HeaderTabIndex == 4)
                        {
                            return;
                        }

                        Progress = NotificationStateService.Progress +1;
                        NotificationStateService.SetOnProgress(Progress);
                        InvokeAsync(StateHasChanged);

                    });
                    await _hubConnection.StartAsync();
                    _draftHubConnection = new HubConnectionBuilder()
                        .WithUrl(ApiSetting.BaseUrl.Replace("/api", "/DraftHub"))
                        .Build();
                    _draftHubConnection.On<SelectedTestDetail>("BroadCastDraftData", (message) =>
                    {
                        if (HeaderTabIndex == 3)
                        {
                            return;
                        }
                        Draft = NotificationStateService.Draft +1;
                        NotificationStateService.SetDraft(Draft);
                        InvokeAsync(StateHasChanged);
                    });
                    await _draftHubConnection.StartAsync();
                    _patientFileHubConnection = new HubConnectionBuilder()
                        .WithUrl(ApiSetting.BaseUrl.Replace("/api", "/PatientFileHub"))
                        .Build();
                    _patientFileHubConnection.On<List<PatientFileDetail>>("BroadCastPatientData", (message) =>
                    {
                        if (HeaderTabIndex == 8)
                        {
                            return;
                        }
                        PatientFile = NotificationStateService.PatientFile +1;
                        NotificationStateService.SetPatientFile(PatientFile);
                        InvokeAsync(StateHasChanged);
                    });
                    await _patientFileHubConnection.StartAsync();
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            var url = NavigationManager.Uri;
            NavigationStateService.OnHeaderTabIndexChanged += OnHeaderTabIndexChanged;
            LabTestCategoryStateServiceService.OnLabTestCategoryChanged += OnLabTestChanged;
            var user = await BearerAuthStateProvider.GetAuthenticationStateAsync();
            if (user.User.Identity is { IsAuthenticated: true })
            {
                if (url.Contains("home"))
                {
                    HeaderTabIndex = 0;
                    Show(HeaderTabIndex);
                }
                else
                {
                    var session = await SessionStoreService.Get();
                    UserName = session?.Authentication?.Username;
                    IsAuthenticated = true;
                    HeaderTabIndex = 1;
                    Show(HeaderTabIndex);
                }

            }
            else
            {
                HeaderTabIndex = 0;
                Show(HeaderTabIndex);
            }
            // else
            // {
            //     while ( await _timer.WaitForNextTickAsync())
            //     {
            //         var result=      DialogService.OpenAsync<Login>("",
            //                 new Dictionary<string, object>(),
            //                 new DialogOptions { Width = "300px", Height = "100%", Style = "position: absolute; Top:40px; right:2px;" });
            //             StateHasChanged();
            //     }
            // }
            await InvokeAsync(StateHasChanged);
        }
        


        private void OnLabTestChanged()
        {
            StateHasChanged();
        }
        private void OnHeaderTabIndexChanged()
        {
            HeaderTabIndex = NavigationStateService.HeaderTabIndex;
        }
        protected void Show(int index)
        {
            NavigationStateService.SetHeaderTabIndex(index);

            switch (index)
            {
                case 0:
                    NavigationManager.NavigateTo("/home");
                    break;
                case 1:
                    NavigationManager.NavigateTo("/lab-test");
                    break;
                case 3:
                    Draft = 0;
                    NotificationStateService.SetDraft(Draft);
                    NavigationManager.NavigateTo("/draft-list");
                    break;
                case 4:
                    Progress = 0;
                    NotificationStateService.SetOnProgress(Progress);
                    NavigationManager.NavigateTo("/on-progress-lab-test");
                    break;
                case 5:
                    NavigationManager.NavigateTo("/completed-lab-test");
                    break;
                case 6:
                    NavigationManager.NavigateTo("/setting-tabs");
                    break;
                case 7:
                    NavigationManager.NavigateTo("/account-setting");
                    break;
                case 8:
                    PatientFile = 0;
                    NotificationStateService.SetPatientFile(PatientFile);
                    NavigationManager.NavigateTo("/patient-files");
                    break;
                case 9:
                    NavigationManager.NavigateTo("/info");
                    break;
            }
            StateHasChanged();
        }
        protected async Task Login()
        {
       var result=     await DialogService.OpenAsync<Login>("",
            new Dictionary<string, object>(),
            new DialogOptions { Width = "300px", Height = "100%",  Style = "position: absolute; Top:80px; right:2px;" });
            StateHasChanged();
        }
        

        protected async Task OnAccount()
        {
            var result=     await DialogService.OpenAsync<Account>("",
                new Dictionary<string, object>(),
                new DialogOptions { Width = "300px", Height = "100%", Style = "position: absolute; Top:80px; right:2px;" });
            StateHasChanged();
            // AuthenticationService.Logout();
            // IsAuthenticated = false;
            // NavigationManager.NavigateTo("/lab-test",true);
            await  InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            DialogService.Dispose();
            LabTestCategoryStateServiceService.OnLabTestCategoryChanged -= OnLabTestChanged;
            NavigationStateService.OnHeaderTabIndexChanged -= OnHeaderTabIndexChanged;
        }
    }
}