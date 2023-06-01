using Domain.Common;
using Domain.Entities;

namespace Domain.Events.SelectedTestDetails;

public class SelectedTestDetailCreatedEvent: BaseEvent
{
    public SelectedTestDetailCreatedEvent(SelectedTestDetail item)
    {
        SelectedTestDetail = item;
    }
    public SelectedTestDetail SelectedTestDetail { get; }
}