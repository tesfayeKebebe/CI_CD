

using Health.Mobile.Server.Enums;
namespace Health.Mobile.Server.Models;
public class PeriodViewModel
{
    public Period Period { get; set; }
    public DateTime? From { get; set; }
    public DateTime? Until { get; set; }
}