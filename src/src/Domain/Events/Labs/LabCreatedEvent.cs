using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class LabCreatedEvent : BaseEvent
{
    public LabCreatedEvent(Lab item)
    {
        Lab = item;
    }

    public Lab Lab { get; }
}
