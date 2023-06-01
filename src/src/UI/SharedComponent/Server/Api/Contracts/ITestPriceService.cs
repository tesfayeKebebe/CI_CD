using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.TestPrices;

namespace SharedComponent.Server.Api.Contracts
{
    public interface ITestPriceService
    {
        Task<List<TestPriceDetail>> GetTestPrice();
        Task<string> CreateTestPrice(TestPrice model);
        Task<string> UpdateTestPrice(TestPriceDetail model);
        Task<string> DeleteTestPrice(string id);
    }
}
