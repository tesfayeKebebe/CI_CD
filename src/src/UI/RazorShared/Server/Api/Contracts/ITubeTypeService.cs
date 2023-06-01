using RazorShared.Server.Models.Tube_Type;
namespace RazorShared.Server.Api.Contracts
{
    public interface ITubeTypeService
    {
        Task<List<TubeTypeDetail>?> GetTubeType();
        Task<string> CreateTubeType(TubeType model);
        Task<string> UpdateTubeType(TubeTypeDetail model);
        Task<string> DeleteTubeType(string id);
    }
}
