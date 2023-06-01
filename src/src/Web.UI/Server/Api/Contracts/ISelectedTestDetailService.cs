using Web.UI.Server.Enums;
using Web.UI.Server.Models.SelectedTestDetails;

namespace Web.UI.Server.Api.Contracts;

public interface ISelectedTestDetailService
{ 
    Task<IEnumerable<SelectedTestDetail>> GetSelectedTestDetails(TestStatus status);
    Task<string> Create(List<SelectedTestDetailModel> models);

}