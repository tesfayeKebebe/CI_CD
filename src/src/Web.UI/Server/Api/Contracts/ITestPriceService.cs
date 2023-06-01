using Web.UI.Server.Models.Labs;
using Web.UI.Server.Models.TestPrices;

namespace Web.UI.Server.Api.Contracts
{
    public interface ITestPriceService
    {
        Task<IList<TestPriceDetail?>> GetTestPrice();
        Task<string> CreateTestPrice(TestPrice model);
        Task<string> UpdateTestPrice(TestPriceDetail model);
        Task<string> DeleteTestPrice(string id);
    }
}
