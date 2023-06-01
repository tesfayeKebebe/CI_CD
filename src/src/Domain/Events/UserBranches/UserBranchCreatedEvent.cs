using Domain.Common;
using Domain.Entities;

namespace Domain.Events.UserBranches;

public class UserBranchCreatedEvent: BaseEvent
{
    public UserBranchCreatedEvent(UserBranch item)
    {
        UserBranch = item;
    }
    public UserBranch UserBranch { get; }
}