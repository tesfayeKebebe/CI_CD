using Web.UI.Server.Models.Categories;
using Web.UI.Server.Models.Labs;
using Web.UI.Server.Models.LabTests;

namespace Web.UI.Server.Api.Contracts
{
    public interface ILabCategoryService
    {
        Task<IList<CategoryDetail?>> GetLabCategory();
        Task<string> CreateLabCategory(Category model);
        Task<string> UpdateLabCategory(CategoryDetail model);
        Task<string> DeleteLabCategory(string id);
    }
}
