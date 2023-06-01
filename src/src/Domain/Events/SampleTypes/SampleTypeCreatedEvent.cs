using Domain.Common;
using Domain.Entities;

namespace Domain.Events.SampleTypes;

public class SampleTypeCreatedEvent: BaseEvent
{
    public SampleTypeCreatedEvent(SampleType item)
    {
        SampleType = item;
    }
    public SampleType SampleType { get; }
}