using System.Text;
using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SharedComponent.Pages.LabTests.TemporaryFileUploads;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.LabTests;
using SharedComponent.Server.Models.UserAssigns;
using SharedComponent.StateManagement;

namespace SharedComponent.Pages.LabTests
{
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject] private ILabTestService LabTestService { get; set; } = null!;
        [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
        private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(30));
        private WindowNavigatorGeolocation _geolocation= null!;
        private GeolocationPosition _geolocationPosition= null!;
        [Inject] private  SessionStoreService SessionStoreService { get; set; }= null!;
        [Inject] private  IUserAssignService UserAssignService { get; set; }= null!;
        [Inject] private  IJSRuntime JsRuntime { get; set; }= null!;
        protected IList<LabTestCategory>? Data { get; set; }
        private IList<LabTestCategory> Originaldata { get; set; } = new List<LabTestCategory>();
        [Inject] private DialogService DialogService { get; set; }= null!;
        protected string? Search { get; set; }
        protected bool ClearEnabled { get; set; }
        protected bool IsAuthenticated = false;
        protected HashSet<string> SelectedLabTests = new HashSet<string>();
        [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
        protected bool IsSpinner = true;
        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();

         
            while (await _timer.WaitForNextTickAsync())
            {
                var session = await SessionStoreService.Get();
                if (session?.Authentication?.Roles == null || !session.Authentication.Roles.Contains("HealthOfficer"))
                {
                    continue;
                }

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
                if (_geolocationPosition == null)
                {
                    continue;
                }

                var userAssign = new UserAssignByService
                {
                    Token = session?.Authentication?.Token,
                    UserId =  Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)),
                    Latitude = _geolocationPosition.Coords.Latitude,
                    Longitude = _geolocationPosition.Coords.Longitude
                };
                await UserAssignService.UpdateUserAssignByService(userAssign);
             
            }
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            IsSpinner=true;
            var session = await SessionStoreService.Get();
            if (session.Authentication != null)
            {
                IsAuthenticated = true;
            }
            SelectedLabTests = LabTestCategoryStateServiceService.SelectedLabTests;
            Originaldata   = await LabTestService.GetLabCategoryTest();
            Data = Originaldata;
            IsSpinner=false;
            await InvokeAsync(StateHasChanged);
        }
        
        protected void OnInputChanged(ChangeEventArgs obj)
        {
            ClearEnabled = !string.IsNullOrEmpty(obj.Value?.ToString());
            Search = obj.Value?.ToString();
            SearchAsync();
            StateHasChanged();
        }
        protected void Clear()
        {
            Search = null;
            ClearEnabled = false;
            Data = Originaldata.ToList();
        }

        protected async Task OpenPatientUpload()
        {
            await DialogService.OpenAsync<PatientFileUpload>("Temporary files ",
                new Dictionary<string, object>()
                {
                    // {"SelectedLab",model} 
                    
                },  
                new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; " });
            // await GetData();
            StateHasChanged();
        }
        private void SearchAsync()
        {
            if(Search==null) return;
            Data = new List<LabTestCategory>();
            foreach (var cat in Originaldata.Where(x => Search != null && x.LabTest.Name.ToLower().Contains(Search.ToLower())).ToList())
            {
                LabTestCategory labTest = new LabTestCategory();
                labTest.Id = cat.Id;
                labTest.Name = cat.Name;
                //labTest.Lab = cat.Lab;
                labTest.LabTest = cat.LabTest;
          
            }
        }



        public void Dispose()
        {

        }
    }
}
