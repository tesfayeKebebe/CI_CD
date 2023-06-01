namespace Application.Business.UserAssigns.Queries
{
    public class UserAssignDetail
    {
        public string UserId { get; set; }= null !;
        public string UserBranchId { get; set; } = null !;
        public string Branch { get; set; } = null!;
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Id { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}
