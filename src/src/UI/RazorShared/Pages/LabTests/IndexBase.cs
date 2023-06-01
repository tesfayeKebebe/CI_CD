using System.Text;
using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Api.Impl;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models.LabTests;
using RazorShared.Server.Models.UserAssigns;
using RazorShared.StateManagement;

namespace RazorShared.Pages.LabTests
{
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject] private ILabTestService LabTestService { get; set; } = null!;
        [Inject] private LabTestCategoryStateService LabTestCategoryStateServiceService { get; set; } = null!;
        private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(2));
        private WindowNavigatorGeolocation Geolocation;
        private GeolocationPosition GeolocationPosition;
        [Inject] private  SessionStoreService _sessionStoreService { get; set; }
        [Inject] private  IUserAssignService _userAssignService { get; set; }
        [Inject] private  IJSRuntime _JsRuntime { get; set; }
        protected IList<LabTestCategory>? Data { get; set; }
        private IList<LabTestCategory> Originaldata { get; set; } = new List<LabTestCategory>();
        protected string? Search { get; set; }
        protected bool ClearEnabled { get; set; }
        protected HashSet<string> SelectedLabTests = new HashSet<string>();
        protected override async Task OnInitializedAsync()
        {
            while (await _timer.WaitForNextTickAsync())
            {
                var session = await _sessionStoreService.Get();
                if (session.Authentication?.Roles == null || !session.Authentication.Roles.Contains("HealthOfficer"))
                {
                    continue;
                }

                var window = await _JsRuntime.Window();
                var navigator = await window.Navigator();
                Geolocation = navigator.Geolocation;
                GeolocationPosition = (await Geolocation.GetCurrentPosition(
                    new PositionOptions
                    {
                        EnableHighAccuracy = true,
                        MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                        TimeoutTimeSpan = TimeSpan.FromMinutes(5)
                    })).Location;
                if (GeolocationPosition == null)
                {
                    continue;
                }

                var userAssign = new UserAssignByService
                {
                    Token = session?.Authentication?.Token,
                    UserId =  Encoding.UTF8.GetString(Convert.FromBase64String(session?.Authentication?.UserId!)),
                    Latitude = GeolocationPosition.Coords.Latitude,
                    Longitude = GeolocationPosition.Coords.Longitude
                };
                await _userAssignService.UpdateUserAssignByService(userAssign);
            }
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {

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
