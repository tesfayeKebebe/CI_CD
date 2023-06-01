using Domain.Common;
using Domain.Entities;

namespace Domain.Events.PatientFiles;

public class PatientFileDeletedEvent: BaseEvent
{
    public PatientFileDeletedEvent(PatientFile item)
    {
        PatientFile = item;
    }
    public PatientFile PatientFile { get; }
}