using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.LabTests;

namespace RazorShared.Server.Api.Contracts
{
    public interface ILabTestService
    {
        Task<IList<LabTestCategory>> GetLabCategoryTest();
        Task<List<LabTestDetail>> GetLabTest();
        Task<string> CreateLabTest(LabTest model);
        Task<string> UpdateLabTest(LabTestDetail model);
        Task<string> DeleteLabTest(string id);
        Task<LabTestDetailById> GetLabTestById(string id);

    }
}
