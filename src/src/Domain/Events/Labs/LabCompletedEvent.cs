using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class LabCompletedEvent : BaseEvent
{
    public LabCompletedEvent(Lab item)
    {
        Lab = item;
    }

    public Lab Lab { get; }
}
