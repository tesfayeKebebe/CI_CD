using RazorShared.Pages.ViewModel;
using RazorShared.Server.Models.Security;

namespace RazorShared.Server.Common
{
    public  class SessionStore
    {
        public  AuthenticationViewModel? Authentication { get; set; }
        public PatientLocation? Location { get; set; }
    }
}
