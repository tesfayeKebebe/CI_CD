using BrowserInterop.Extensions;
using BrowserInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models;
using SharedComponent.Server.Models.SelectedTestStatuses;

namespace SharedComponent.Pages.LabTests;

public class MapRouteBase: ComponentBase
{
    [Inject] private IJSRuntime JsRuntime { get; set; }= null!;
    [Inject] SessionStoreService SessionStoreService { get; set; }= null!;
    [Inject] DialogService DialogService { get; set; }= null!;
    [Inject] ISelectedTestStatusService SelectedTestStatusService { get; set; }= null!;
    [Inject] NotificationService NotificationService { get; set; }= null!;
    private WindowNavigatorGeolocation _geolocation= null!;
    private GeolocationPosition _geolocationPosition= null!;
    protected SessionStore SessionStore = new SessionStore();
    protected UpdateSelectedTestSample Model = new UpdateSelectedTestSample();
    [Inject] NavigationManager NavigationManager { get; set; }= null!;
    private Lazy<IJSObjectReference> _routeModule = new();
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(2));
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        SessionStore = await SessionStoreService.Get();
        Model.TransactionNumber = SessionStore?.Location?.TransactionNumber;
        Model.IsSampleTaken = (bool)SessionStore?.Location?.IsSampleTaken;
    }
    protected async Task OnSubmit()
    {
      
        try
        {
            IsSpinner=true;
            Model.TransactionNumber = SessionStore?.Location?.TransactionNumber;
            var result= await SelectedTestStatusService.UpdateIsSample(Model);
            NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            IsSpinner=false;
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
            IsSpinner=true;
            var window = await JsRuntime.Window();
            var navigator = await window.Navigator();
            _geolocation = navigator.Geolocation;
            _geolocationPosition = (await _geolocation.GetCurrentPosition(
                new PositionOptions()
                {
                    EnableHighAccuracy = true,
                    MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                    TimeoutTimeSpan = TimeSpan.FromMinutes(5)
                })).Location;
            if (SessionStore.Location != null)
            {
                _routeModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
                await _routeModule.Value.InvokeVoidAsync("mapFunction", SessionStore.Location.Latitude, SessionStore.Location.Longitude);
                //await RouteModule.Value.InvokeVoidAsync("tomTomMapFunction",SessionStore.Location.Latitude,SessionStore.Location.Longitude,9.0117,38.7535);

                while (await _timer.WaitForNextTickAsync())
                {
                    await _routeModule.Value.InvokeVoidAsync("currentLocation");
                }
              
            }
            IsSpinner=false;
        }
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
            new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true, CloseDialogOnOverlayClick = true,Style = "position: absolute; Top:80px; "  });
        StateHasChanged();
    }

    [JSInvokable("GetCurrentLocation")]
    public  async Task<LocationViewModel> GetCurrentLocation()
    {
        var window = await JsRuntime.Window();
        var navigator = await window.Navigator();
        _geolocation = navigator.Geolocation;
        _geolocationPosition = (await _geolocation.GetCurrentPosition(
            new PositionOptions()
            {
                EnableHighAccuracy = true,
                MaximumAgeTimeSpan = TimeSpan.FromHours(24),
                TimeoutTimeSpan = TimeSpan.FromMinutes(5)
            })).Location;
        return new LocationViewModel
        {
            Latitude = _geolocationPosition.Coords.Latitude,
            Longitude = _geolocationPosition.Coords.Longitude
        };
    }
}