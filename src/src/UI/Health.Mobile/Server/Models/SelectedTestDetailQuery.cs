using Health.Mobile.Server.Enums;

namespace Health.Mobile.Server.Models;

public class SelectedTestDetailQuery
{
    public TestStatus TestStatus { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}