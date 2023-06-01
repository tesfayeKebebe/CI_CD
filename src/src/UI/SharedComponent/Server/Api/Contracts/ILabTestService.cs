using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.LabTests;

namespace SharedComponent.Server.Api.Contracts
{
    public interface ILabTestService
    {
        Task<IList<LabTestCategory>> GetLabCategoryTest();
        Task<List<LabTestDetail>> GetLabTest();
        Task<List<LabTestPriceDetail>> GetLabTestsDropDown();
        Task<string> CreateLabTest(LabTest model);
        Task<string> UpdateLabTest(LabTestDetail model);
        Task<string> DeleteLabTest(string id);
        Task<LabTestDetailById> GetLabTestById(string id);

    }
}
