using Domain.Common;
using Domain.Entities;

namespace Domain.Events.SelectedTestDetails;

public class SelectedTestDetailDeletedEvent: BaseEvent
{
    public SelectedTestDetailDeletedEvent(SelectedTestDetail item)
    {
        SelectedTestDetail = item;
    }
    public SelectedTestDetail SelectedTestDetail { get; }
}