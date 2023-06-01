using Application.Business.PatientFiles.Queries;
using Microsoft.AspNetCore.SignalR;

namespace Application.Notifications;

public class PatientFileHub: Hub<IPatientFileHub>
{
    public async Task SendMessage(IEnumerable<PatientFileDetail> patientFileDetails)
    {
        await Clients.All.BroadCastPatientData(patientFileDetails);
    }
}