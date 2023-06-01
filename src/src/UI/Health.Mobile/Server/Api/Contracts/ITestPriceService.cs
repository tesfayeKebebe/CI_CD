using Health.Mobile.Server.Models.Labs;
using Health.Mobile.Server.Models.TestPrices;

namespace Health.Mobile.Server.Api.Contracts
{
    public interface ITestPriceService
    {
        Task<List<TestPriceDetail>> GetTestPrice();
        Task<string> CreateTestPrice(TestPrice model);
        Task<string> UpdateTestPrice(TestPriceDetail model);
        Task<string> DeleteTestPrice(string id);
    }
}
