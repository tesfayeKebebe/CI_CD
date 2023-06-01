using SharedComponent.Server.Models.SelectedTestStatuses;

namespace SharedComponent.Server.Api.Contracts;

public interface ISelectedTestStatusService
{
   Task<string> Update(SelectedTestStatus command);
   Task<string> AssignUser(UpdateAssignedUser model);
   Task<string> UpdateIsSample(UpdateSelectedTestSample model);

}