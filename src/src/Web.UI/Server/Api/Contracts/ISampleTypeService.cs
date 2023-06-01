using Web.UI.Server.Models.Sample_Type;

namespace Web.UI.Server.Api.Contracts
{
    public interface ISampleTypeService
    {
        Task<IList<SampleTypeDetail?>> GetSampleType();
        Task<string> CreateSampleType(SampleType model);
        Task<string> UpdateSampleType(SampleTypeDetail model);
        Task<string> DeleteSampleType(string id);
    }
}
