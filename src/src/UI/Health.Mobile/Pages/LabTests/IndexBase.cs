using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Health.Mobile.Pages.LabTests.TemporaryFileUploads;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.LabTests;
using Health.Mobile.Server.Models.UserAssigns;
using Health.Mobile.StateManagement;
using Health.Mobile.Server.Common;

namespace Health.Mobile.Pages.LabTests
{
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject] private ILabTestService LabTestService { get; set; } = null!;
        [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
        private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(30));
        [Inject] private  SessionStoreService _sessionStoreService { get; set; } = null!;
        [Inject] private  IUserAssignService _userAssignService { get; set; } = null!;
        [Inject] private  IJSRuntime _JsRuntime { get; set; } = null!;
        protected IList<LabTestCategory>? Data { get; set; }
        private IList<LabTestCategory> Originaldata { get; set; } = new List<LabTestCategory>();
        [Inject] private DialogService DialogService { get; set; } = null!;
        protected string? Search { get; set; }
        protected bool ClearEnabled { get; set; }
        protected bool IsAuthenticated = false;
        protected HashSet<string> SelectedLabTests = new HashSet<string>();
        [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
        [Inject] private LocationInformationService LocationInformationService { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();

         
            while (await _timer.WaitForNextTickAsync())
            {
                var session = await _sessionStoreService.Get();
                if (session?.Authentication?.Roles == null || !session.Authentication.Roles.Contains("HealthOfficer"))
                {
                    continue;
                }

                var location = await LocationInformationService.CheckAndRequestLocationPermission();
                if (location == null)
                {
                    continue;
                }

                var userAssign = new UserAssignByService
                {
                    Token = session?.Authentication?.Token,
                    UserId =  Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)),
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };
                await _userAssignService.UpdateUserAssignByService(userAssign);
            }
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var session = await _sessionStoreService.Get();
            if (session.Authentication != null)
            {
                IsAuthenticated = true;
            }
            SelectedLabTests = LabTestCategoryStateServiceService.SelectedLabTests;
            Originaldata   = await LabTestService.GetLabCategoryTest();
            Data = Originaldata;
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
                new DialogOptions { Width = "95%", Height = "90%", Style = "position: absolute; Top:80px; " });
            // await GetData();
            StateHasChanged();
        }
        private void SearchAsync()
        {
            if(Search==null) return;
            Data = new List<LabTestCategory>();
            foreach (var cat in Originaldata)
            {
                LabTestCategory labTest = new LabTestCategory();
                labTest.Id = cat.Id;
                labTest.Name = cat.Name;
                labTest.Lab = cat.Lab;
                labTest.LabTests = new List<LabTestPriceDetail>();
                foreach (var test in cat.LabTests.Where(x=>Search != null && x.Name.ToLower().Contains(Search.ToLower() )).ToList())
                {
                    labTest.LabTests.Add(test);
                }
                Data.Add(labTest);
            }
        }



        public void Dispose()
        {

        }
    }
}
