
using Application.Business.PatientFiles.Queries;

namespace Application.Notifications;

public interface IPatientFileHub
{
    Task BroadCastPatientData(IEnumerable<PatientFileDetail> patientFileDetails);
}