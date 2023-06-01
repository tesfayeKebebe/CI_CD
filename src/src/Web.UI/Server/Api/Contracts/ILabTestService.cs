using Web.UI.Server.Models.Labs;
using Web.UI.Server.Models.LabTests;

namespace Web.UI.Server.Api.Contracts
{
    public interface ILabTestService
    {
        Task<IList<LabTestCategory>> GetLabCategoryTest();
        Task<List<LabTestDetail>> GetLabTest();
        Task<string> CreateLabTest(LabTest model);
        Task<string> UpdateLabTest(LabTestDetail model);
        Task<string> DeleteLabTest(string id);

    }
}
