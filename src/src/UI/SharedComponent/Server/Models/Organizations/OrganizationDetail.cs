using SharedComponent.Server.Models.BankAccounts;

namespace SharedComponent.Server.Models.Organizations
{
    public class OrganizationDetail
    {
        public string About { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string LastModifiedBy { get; set; }= null!;
        public  string? Telephone { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
        public List<BankAccountDetail> AccountDetails { get; set; } = new List<BankAccountDetail>();

    }
}
