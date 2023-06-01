using Domain.Common;
using Domain.Entities;

namespace Domain.Events.UserAssigns;

public class UserAssignCreatedEvent: BaseEvent
{
    public UserAssignCreatedEvent(UserAssign item)
    {
        UserAssign = item;
    }
    public UserAssign UserAssign { get; }
}