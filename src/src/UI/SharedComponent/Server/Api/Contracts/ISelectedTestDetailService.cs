using SharedComponent.Server.Enums;
using SharedComponent.Server.Models;
using SharedComponent.Server.Models.SelectedTestDetails;

namespace SharedComponent.Server.Api.Contracts;

public interface ISelectedTestDetailService
{ 
    Task<List<SelectedTestDetail>> GetSelectedTestDetails(TestStatus status);
    Task<string> Create(List<SelectedTestDetailModel> models, TestStatus status, double latitude, double longitude);
    Task<IEnumerable<SelectedLabTestDetailByParentId>> GetLabTestById(string parentId);
    Task<List<SelectedTestDetail>> GetSelectedTestDetailsByDate(SelectedTestDetailQuery query);
    Task<List<SelectedTestDetail>> GetSelectedTestDetailsByUser();
    Task<string> DeleteSelectedDetailByTransaction(string transaction);

}