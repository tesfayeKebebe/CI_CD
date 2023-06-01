using Domain.Common;
using Domain.Entities;

namespace Domain.Events.PatientFiles;
public class PatientFileCreatedEvent: BaseEvent
{
    public PatientFileCreatedEvent(PatientFile item)
    {
        PatientFile = item;
    }
    public PatientFile PatientFile { get; }
}