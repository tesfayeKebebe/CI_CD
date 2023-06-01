using Health.Mobile.Server.Models.SelectedTestStatuses;

namespace Health.Mobile.Server.Api.Contracts;

public interface ISelectedTestStatusService
{
   Task<string> Update(SelectedTestStatus command);
   Task<string> AssignUser(UpdateAssignedUser model);
   Task<string> UpdateIsSample(UpdateSelectedTestSample model);

}