using Health.Mobile.Pages.ViewModel;
using Health.Mobile.Server.Models.LabTestResults;
using Health.Mobile.Server.Models.Security;

namespace Health.Mobile.Server.Common
{
    public  class SessionStore
    {
        public  AuthenticationViewModel? Authentication { get; set; }
        public PatientLocation? Location { get; set; }
    }
}
