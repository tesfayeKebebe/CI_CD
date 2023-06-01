using Domain.Common;
using Domain.Entities;

namespace Domain.Events.LabTestResults;

public class LabTestResultCreatedEvent : BaseEvent
{
    public LabTestResultCreatedEvent(TestResult item)
    {
        TestResult = item;
    }
    public TestResult TestResult { get; }
}
