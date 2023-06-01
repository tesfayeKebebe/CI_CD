using Web.UI.Server.Models.SelectedTestStatuses;

namespace Web.UI.Server.Api.Contracts;

public interface ISelectedTestStatusService
{
   Task<string> Update(SelectedTestStatus command);

}