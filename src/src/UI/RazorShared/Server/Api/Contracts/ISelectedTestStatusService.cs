using RazorShared.Server.Models.SelectedTestStatuses;

namespace RazorShared.Server.Api.Contracts;

public interface ISelectedTestStatusService
{
   Task<string> Update(SelectedTestStatus command);
   Task<string> AssignUser(UpdateAssignedUser model);
   Task<string> UpdateIsSample(UpdateSelectedTestSample model);

}