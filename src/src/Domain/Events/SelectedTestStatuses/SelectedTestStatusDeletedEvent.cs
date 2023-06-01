using Domain.Common;
using Domain.Entities;

namespace Domain.Events.SelectedTestStatuses;

public class SelectedTestStatusDeletedEvent: BaseEvent
{
    public SelectedTestStatusDeletedEvent(SelectedTestStatus item)
    {
        SelectedTestStatus = item;
    }
    public SelectedTestStatus SelectedTestStatus { get; }
    
}