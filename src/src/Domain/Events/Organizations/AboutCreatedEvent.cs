using Domain.Common;
using Domain.Entities;

namespace Domain.Events.Organizations;

public class OrganizationCreatedEvent: BaseEvent
{
    public OrganizationCreatedEvent(Organization item)
    {
        Organization = item;
    }
    public Organization Organization { get; }
    
}