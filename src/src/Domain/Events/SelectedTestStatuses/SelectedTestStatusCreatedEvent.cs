using Domain.Common;
using Domain.Entities;

namespace Domain.Events.SelectedTestStatuses;

public class SelectedTestStatusCreatedEvent: BaseEvent
{
    public SelectedTestStatusCreatedEvent(SelectedTestStatus item)
    {
        SelectedTestStatus = item;
    }
    public SelectedTestStatus SelectedTestStatus { get; }
    
}