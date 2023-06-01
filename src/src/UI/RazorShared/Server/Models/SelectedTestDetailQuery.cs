using RazorShared.Server.Enums;

namespace RazorShared.Server.Models;

public class SelectedTestDetailQuery
{
    public TestStatus TestStatus { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}