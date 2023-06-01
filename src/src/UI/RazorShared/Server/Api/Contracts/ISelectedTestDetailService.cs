using RazorShared.Server.Enums;
using RazorShared.Server.Models;
using RazorShared.Server.Models.SelectedTestDetails;

namespace RazorShared.Server.Api.Contracts;

public interface ISelectedTestDetailService
{ 
    Task<List<SelectedTestDetail>> GetSelectedTestDetails(TestStatus status);
    Task<string> Create(List<SelectedTestDetailModel> models, TestStatus status, double latitude, double longitude);
    Task<IEnumerable<SelectedLabTestDetailByParentId>> GetLabTestById(string parentId);
    Task<List<SelectedTestDetail>> GetSelectedTestDetailsByDate(SelectedTestDetailQuery query);
    Task<List<SelectedTestDetail>> GetSelectedTestDetailsByUser();

}