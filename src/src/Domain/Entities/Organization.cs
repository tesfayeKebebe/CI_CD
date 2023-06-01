using Domain.Common;

namespace Domain.Entities;

public class Organization:  BaseAuditableEntity
{
    public required string About { get; set; }
    public required string Telephone { get; set; }
    public string? Location { get; set; }
    public string? Email { get; set; }
}