using RazorShared.Server.Models.Categories;
using RazorShared.Server.Models.Labs;
using RazorShared.Server.Models.LabTests;

namespace RazorShared.Server.Api.Contracts
{
    public interface ILabCategoryService
    {
        Task<List<CategoryDetail>?> GetLabCategory();
        Task<string> CreateLabCategory(Category model);
        Task<string> UpdateLabCategory(CategoryDetail model);
        Task<string> DeleteLabCategory(string id);
    }
}
