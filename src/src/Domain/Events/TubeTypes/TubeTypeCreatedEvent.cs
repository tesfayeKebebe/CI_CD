using Domain.Common;
using Domain.Entities;

namespace Domain.Events.TubeTypes;

public class TubeTypeCreatedEvent: BaseEvent
{
    public TubeTypeCreatedEvent(TubeType item)
    {
        TubeType = item;
    }

    public TubeType TubeType { get; }
}