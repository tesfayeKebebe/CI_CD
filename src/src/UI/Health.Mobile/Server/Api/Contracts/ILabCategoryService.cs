using Health.Mobile.Server.Models.Categories;
using Health.Mobile.Server.Models.Labs;
using Health.Mobile.Server.Models.LabTests;

namespace Health.Mobile.Server.Api.Contracts
{
    public interface ILabCategoryService
    {
        Task<List<CategoryDetail>?> GetLabCategory();
        Task<string> CreateLabCategory(Category model);
        Task<string> UpdateLabCategory(CategoryDetail model);
        Task<string> DeleteLabCategory(string id);
    }
}
