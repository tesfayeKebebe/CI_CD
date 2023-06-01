using Web.UI.Server.Models.Tube_Type;
namespace Web.UI.Server.Api.Contracts
{
    public interface ITubeTypeService
    {
        Task<IList<TubeTypeDetail?>> GetTubeType();
        Task<string> CreateTubeType(TubeType model);
        Task<string> UpdateTubeType(TubeTypeDetail model);
        Task<string> DeleteTubeType(string id);
    }
}
