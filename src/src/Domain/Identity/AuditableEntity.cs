namespace Domain.Identity;

public class AuditableEntity : IAuditableEntity
{
    public string? CreatedUserId { get; set; }
    public string? UpdatedUserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
   
}