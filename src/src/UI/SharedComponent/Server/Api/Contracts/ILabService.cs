using SharedComponent.Server.Models.Labs;

namespace SharedComponent.Server.Api.Contracts
{
    public interface ILabService
    {
        Task<List<LabDetail>?> GetLab();
        Task<string> CreateLab(Lab model);
        Task<string> UpdateLab(LabDetail model);
        Task<string> DeleteLab(string id);
    }
}
