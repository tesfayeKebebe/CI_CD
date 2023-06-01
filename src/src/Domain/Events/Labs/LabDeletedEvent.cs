using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class LabDeletedEvent : BaseEvent
{
    public LabDeletedEvent(Lab item)
    {
        Lab = item;
    }

    public Lab Lab { get; }
}
