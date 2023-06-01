using SharedComponent.Pages.ViewModel;
using SharedComponent.Server.Models.LabTestResults;
using SharedComponent.Server.Models.Security;

namespace SharedComponent.Server.Common
{
    public  class SessionStore
    {
        public  AuthenticationViewModel? Authentication { get; set; }
        public PatientLocation? Location { get; set; }
    }
}
