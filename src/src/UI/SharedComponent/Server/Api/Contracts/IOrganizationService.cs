using SharedComponent.Server.Models.Organizations;

namespace SharedComponent.Server.Api.Contracts;

public interface IOrganizationService
{
    Task<OrganizationDetail?> GetOrganization();
    Task<string> CreateOrganization(OrganizationDetail? model);
    Task<string> UpdateOrganization(OrganizationDetail? model);
}