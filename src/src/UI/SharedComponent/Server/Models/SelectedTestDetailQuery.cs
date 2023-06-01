using SharedComponent.Server.Enums;

namespace SharedComponent.Server.Models;

public class SelectedTestDetailQuery
{
    public TestStatus TestStatus { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}