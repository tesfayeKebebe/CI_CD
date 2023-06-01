using Health.Mobile.Server.Enums;
using Health.Mobile.Server.Models;
using Health.Mobile.Server.Models.SelectedTestDetails;

namespace Health.Mobile.Server.Api.Contracts;

public interface ISelectedTestDetailService
{ 
    Task<List<SelectedTestDetail>> GetSelectedTestDetails(TestStatus status);
    Task<string> Create(List<SelectedTestDetailModel> models, TestStatus status, double latitude, double longitude);
    Task<IEnumerable<SelectedLabTestDetailByParentId>> GetLabTestById(string parentId);
    Task<List<SelectedTestDetail>> GetSelectedTestDetailsByDate(SelectedTestDetailQuery query);
    Task<List<SelectedTestDetail>> GetSelectedTestDetailsByUser();
    Task<string> DeleteSelectedDetailByTransaction(string transaction);

}