
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Health.Mobile.Pages.ViewModel;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models;
using Health.Mobile.Server.Models.SelectedTestStatuses;

namespace Health.Mobile.Pages.LabTests;

public class MapRouteBase: ComponentBase
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] SessionStoreService SessionStoreService { get; set; } = null!;
    [Inject] DialogService DialogService { get; set; } = null!;
    [Inject] ISelectedTestStatusService SelectedTestStatusService { get; set; } = null!;
    [Inject] NotificationService NotificationService { get; set; } = null!;
    protected SessionStore SessionStore = new SessionStore();
    protected UpdateSelectedTestSample Model = new UpdateSelectedTestSample();
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    private Lazy<IJSObjectReference> RouteModule = new();
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(2));
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    [Inject] private LocationInformationService LocationInformationService { get; set; } = null!;
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
         

            if (SessionStore.Location != null)
            {
                RouteModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
                await RouteModule.Value.InvokeVoidAsync("mapFunction",SessionStore.Location.Latitude,SessionStore.Location.Longitude,9.0117,38.7535);
                //await RouteModule.Value.InvokeVoidAsync("tomTomMapFunction",SessionStore.Location.Latitude,SessionStore.Location.Longitude,9.0117,38.7535);

                while (await _timer.WaitForNextTickAsync())
                {
                    await RouteModule.Value.InvokeVoidAsync("currentLocation");
                }
            }
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
            new DialogOptions { Width = "50%", Height = "90%", Style = "position: absolute; Top:80px; " });
        StateHasChanged();
    }

    [JSInvokable("GetCurrentLocation")]
    public  async Task<LocationViewModel> GetCurrentLocation()
    {
        var location = await LocationInformationService.CheckAndRequestLocationPermission();
        if (location == null) return new LocationViewModel();
        return new LocationViewModel
        {
            Latitude = location.Latitude,
            Longitude = location.Longitude
        };
    }
}