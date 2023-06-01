using Domain.Common;

namespace Domain.Entities;

public class BankAccount:  BaseAuditableEntity
{
    public required string Name { get; set; }
    public long Account { get; set; }
}