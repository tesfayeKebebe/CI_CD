﻿using SharedComponent.Server.Models.Sample_Type;

namespace SharedComponent.Server.Api.Contracts
{
    public interface ISampleTypeService
    {
        Task<List<SampleTypeDetail>?> GetSampleType();
        Task<string> CreateSampleType(SampleType model);
        Task<string> UpdateSampleType(SampleTypeDetail model);
        Task<string> DeleteSampleType(string id);
    }
}
