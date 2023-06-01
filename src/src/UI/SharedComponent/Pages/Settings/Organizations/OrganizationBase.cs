using System.Net.Security;
using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Models.Organizations;

namespace SharedComponent.Pages.Settings.Organizations;

public class OrganizationBase: ComponentBase
{
    protected OrganizationDetail? Model { get; set; } = new OrganizationDetail();
    [Inject] private IOrganizationService OrganizationService { get; set; } = null!;
    [Inject] private  NotificationService NotificationService { get; set; }= null!;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Model=await GetData();
        IsSpinner=false;
    }
    private async Task<OrganizationDetail?> GetData()
    {
        return await OrganizationService.GetOrganization();
    }

    protected async Task OnSubmit(OrganizationDetail? detail)
    {
        try
        {
            IsSpinner=true;
            if (detail.Id != null)
            {
                var result = await OrganizationService.UpdateOrganization(detail); 
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            }
            else
            {
                var result = await OrganizationService.CreateOrganization(detail);
                Model=  await GetData();
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
                StateHasChanged();
            }

            IsSpinner=false;
        }
        catch (Exception e)
        {
            IsSpinner=false;
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }

        
    }
}