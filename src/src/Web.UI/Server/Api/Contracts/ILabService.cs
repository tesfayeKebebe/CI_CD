using Web.UI.Server.Models.Labs;

namespace Web.UI.Server.Api.Contracts
{
    public interface ILabService
    {
        Task<IList<LabDetail?>> GetLab();
        Task<string> CreateLab(Lab model);
        Task<string> UpdateLab(LabDetail model);
        Task<string> DeleteLab(string id);
    }
}
