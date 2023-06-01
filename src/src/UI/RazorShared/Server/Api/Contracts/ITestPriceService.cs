using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.TestPrices;

namespace RazorShared.Server.Api.Contracts
{
    public interface ITestPriceService
    {
        Task<List<TestPriceDetail>> GetTestPrice();
        Task<string> CreateTestPrice(TestPrice model);
        Task<string> UpdateTestPrice(TestPriceDetail model);
        Task<string> DeleteTestPrice(string id);
    }
}
