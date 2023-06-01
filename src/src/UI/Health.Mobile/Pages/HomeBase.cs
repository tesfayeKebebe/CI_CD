using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Health.Mobile.Pages.Securities;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Common.Services;
using Health.Mobile.Server.Models.Organizations;

namespace Health.Mobile.Pages;

public class HomeBase: ComponentBase
{
    [Inject] private IJSRuntime JsRuntime { get; set; }= null!;
    private Lazy<IJSObjectReference> _routeModule = new();
    protected OrganizationDetail? Model { get; set; } = new OrganizationDetail();
    [Inject] private IOrganizationService OrganizationService { get; set; } = null!;
    [Inject] private BearerAuthStateProvider BearerAuthStateProvider { get; set; } = null!;
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
        _routeModule = new(await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Script.js"));
        await _routeModule.Value.InvokeVoidAsync("automatic");
    }
    
}