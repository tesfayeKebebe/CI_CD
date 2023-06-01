using Health.Mobile.Server.Models.Labs;
using Health.Mobile.Server.Models.LabTests;

namespace Health.Mobile.Server.Api.Contracts
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
