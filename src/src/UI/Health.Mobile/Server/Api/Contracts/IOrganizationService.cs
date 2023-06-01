using Health.Mobile.Server.Models.Organizations;

namespace Health.Mobile.Server.Api.Contracts;

public interface IOrganizationService
{
    Task<OrganizationDetail?> GetOrganization();
    Task<string> CreateOrganization(OrganizationDetail? model);
    Task<string> UpdateOrganization(OrganizationDetail? model);
}