using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Identity;
public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public ApplicationUser()
    {
        Roles =new HashSet<IdentityUserRole<string>>();
        Claims =new HashSet<IdentityUserClaim<string>>();
    }
    public virtual string FriendlyName
    {
        get
        {
            string friendlyName = string.IsNullOrWhiteSpace(FullName) ? UserName : FullName;

            //if (!string.IsNullOrWhiteSpace(TIN))
            //    friendlyName = $"{TIN} {friendlyName}";

            return friendlyName;
        }
    }

    public string? IdNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Configuration { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsExisting { get; set; }
    public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

    public string? CreatedUserId { get; set; }
    public string? UpdatedUserId { get; set; }
    [Column(TypeName = "datetime2")]
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    [Column(TypeName = "datetime2")]
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public string? RefreshToken { get; set; }
    [Column(TypeName = "datetime2")]
    public DateTime? RefreshTokenExpiryTime { get; set; }
    /// <summary>
    /// Navigation property for the roles this user belongs to.
    /// </summary>
    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
    /// <summary>
    /// Navigation property for the claims this user possesses.
    /// </summary>
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
}


