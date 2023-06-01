using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models;
using RazorShared.Server.Models.SelectedTestStatuses;

namespace RazorShared.Pages.LabTests;

public class MapRouteBase: ComponentBase
{
    [Inject] private IJSRuntime JsRuntime { get; set; }
    [Inject] SessionStoreService SessionStoreService { get; set; }
    [Inject] DialogService DialogService { get; set; }
    [Inject] ISelectedTestStatusService SelectedTestStatusService { get; set; }
    [Inject] NotificationService NotificationService { get; set; }
    private WindowNavigatorGeolocation Geolocation;
    private GeolocationPosition GeolocationPosition;
    protected SessionStore SessionStore = new SessionStore();
    protected UpdateSelectedTestSample Model = new UpdateSelectedTestSample();
    [Inject] NavigationManager NavigationManager { get; set; }
    private Lazy<IJSObjectReference> RouteModule = new();
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(2));
    protected override async Task OnInitializedAsync()
    {
        SessionStore = await SessionStoreService.Get();
        Model.TransactionNumber = SessionStore?.Location?.TransactionNumber;
        Model.IsSampleTaken = (bool) SessionStore?.Location?.IsSampleTaken;
    }
    protected async Task OnSubmit()
    {
      
        try
        {
            Model.TransactionNumber = SessionStore?.Location?.TransactionNumber;
            var result= await SelectedTestStatusService.UpdateIsSample(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            NavigationManager.NavigateTo("/on-progress-lab-test");
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
    

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var window = await JsRuntime.Window();
            var navigator = await window.Navigator();
            Geolocation = navigator.Geolocation;
            GeolocationPosition = (await Geolocation.GetCurrentPosition(
                new PositionOptions()
                {
                    EnableHighAccuracy = true,
                    MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                    TimeoutTimeSpan = TimeSpan.FromMinutes(5)
                })).Location;

            if (SessionStore.Location != null)
            {
                RouteModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/MapRoute.js"));
                await RouteModule.Value.InvokeVoidAsync("mapFunction",SessionStore.Location.Latitude,SessionStore.Location.Longitude,9.0117,38.7535);
                while (await _timer.WaitForNextTickAsync())
                {
                    try
                    {
                        await RouteModule.Value.InvokeVoidAsync("currentLocation");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                  
                }
            }
        }
    }
   protected async Task OnAllowLocation()
    {
        await RouteModule.Value.InvokeVoidAsync("currentLocation");
    }
    protected async Task OnDetail()
    {
        var model = new SelectedLabTestDetailViewModel
        {
            Name = SessionStore?.Location?.PatientName,
            TransactionNumber = SessionStore?.Location?.TransactionNumber
        };
        await DialogService.OpenAsync<SelectedLabTestDetail>("Detail",
            new Dictionary<string, object>() { {"SelectedLab",model} },  
            new DialogOptions { Width = "50%", Height = "90%"  });
        StateHasChanged();
    }

    [JSInvokable("GetCurrentLocation")]
    public  async Task<LocationViewModel> GetCurrentLocation()
    {

        var window = await JsRuntime.Window();
        var navigator = await window.Navigator();
        Geolocation = navigator.Geolocation;
        GeolocationPosition = (await Geolocation.GetCurrentPosition(
            new PositionOptions()
            {
                EnableHighAccuracy = true,
                MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                TimeoutTimeSpan = TimeSpan.FromMinutes(5)
            })).Location;
        return new LocationViewModel
        {
            Latitude = GeolocationPosition.Coords.Latitude,
            Longitude = GeolocationPosition.Coords.Longitude
        };
    }
}