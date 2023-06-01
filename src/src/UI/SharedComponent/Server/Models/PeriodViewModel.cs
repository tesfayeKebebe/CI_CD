

using SharedComponent.Server.Enums;
namespace SharedComponent.Server.Models;
public class PeriodViewModel
{
    public Period Period { get; set; }
    public DateTime? From { get; set; }
    public DateTime? Until { get; set; }
}