using Health.Mobile.Server.Models.PatientFiles;

namespace Health.Mobile.Server.Api.Contracts;

public interface IPatientFileService
{

    Task<List<PatientFileDetail>> GetPatientFiles();
    Task<IList<PatientFileDetail>> GetPatientFilesByPatientId();
    Task<string> Create(List<PatientFileModel> models);
    Task<string> Update(PatientFileDetail model);
    Task<string> Delete(string id);
    Task<string> DeleteByPatientId(string patientId);
}

     