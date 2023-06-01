using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SharedComponent.Pages.Securities;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Organizations;

namespace SharedComponent.Pages;

public class HomeBase: ComponentBase, IDisposable
{
    [Inject] private IJSRuntime JsRuntime { get; set; }= null!;
    private Lazy<IJSObjectReference> _routeModule = new();
    protected OrganizationDetail? Model { get; set; } = new OrganizationDetail();
    [Inject] private IOrganizationService OrganizationService { get; set; } = null!;
    [Inject] private BearerAuthStateProvider BearerAuthStateProvider { get; set; } = null!;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(30));
    protected bool IsAuthenticated { get; set; }
    protected override async Task OnInitializedAsync()
    {
        var user =await BearerAuthStateProvider.GetAuthenticationStateAsync();
        if (user.User.Identity is {IsAuthenticated: true})
        {
            IsAuthenticated = true;
        }
        Model=await GetData();
        StateHasChanged();
    }
    private async Task<OrganizationDetail?> GetData()
    {
        return await OrganizationService.GetOrganization();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _routeModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
            int counter = 1;
            while (await _timer.WaitForNextTickAsync())
            {
                await _routeModule.Value.InvokeVoidAsync("automatic", counter);
                counter++;
                if (counter > 4)
                {
                    counter = 1;
                }
            }
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}