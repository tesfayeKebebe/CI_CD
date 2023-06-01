using RazorShared.Server.Models.Labs;

namespace RazorShared.Server.Api.Contracts
{
    public interface ILabService
    {
        Task<List<LabDetail>?> GetLab();
        Task<string> CreateLab(Lab model);
        Task<string> UpdateLab(LabDetail model);
        Task<string> DeleteLab(string id);
    }
}
