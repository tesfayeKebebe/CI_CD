using Application.Business.BankAccounts.Queries;

namespace Application.Business.Organizations.Queries
{
    public class OrganizationDetail
    {
        public string? About { get; set; } = null!;
        public string Id { get; set; } = null!;
        public  string? Telephone { get; set; } = null!;
        public string? Location { get; set; } 
        public string? Email { get; set; }
        public List<BankAccountDetail> AccountDetails { get; set; } = new List<BankAccountDetail>();

    }
    
}
