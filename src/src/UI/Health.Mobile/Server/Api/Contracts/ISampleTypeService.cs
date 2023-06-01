using Health.Mobile.Server.Models.Sample_Type;

namespace Health.Mobile.Server.Api.Contracts
{
    public interface ISampleTypeService
    {
        Task<List<SampleTypeDetail>?> GetSampleType();
        Task<string> CreateSampleType(SampleType model);
        Task<string> UpdateSampleType(SampleTypeDetail model);
        Task<string> DeleteSampleType(string id);
    }
}
