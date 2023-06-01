using SharedComponent.Server.Models.Categories;
using SharedComponent.Server.Models.Labs;
using SharedComponent.Server.Models.LabTests;

namespace SharedComponent.Server.Api.Contracts
{
    public interface ILabCategoryService
    {
        Task<List<CategoryDetail>?> GetLabCategory();
        Task<string> CreateLabCategory(Category model);
        Task<string> UpdateLabCategory(CategoryDetail model);
        Task<string> DeleteLabCategory(string id);
    }
}
