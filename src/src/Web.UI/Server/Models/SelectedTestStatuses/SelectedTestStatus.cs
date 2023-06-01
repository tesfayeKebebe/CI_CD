using Web.UI.Server.Enums;
namespace Web.UI.Server.Models.SelectedTestStatuses;
public class SelectedTestStatus
{
    public string ParentId { get; set; } = null!;
    public TestStatus TestStatus { get; set; }
}
