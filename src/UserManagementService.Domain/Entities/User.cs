using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Domain.Entities;

public class User : BaseEntity
{
    [MaxLength(50)]
    public string FirstName { get; private set; } = string.Empty;

    [MaxLength(50)]
    public string LastName { get; private set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public required string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public required string UserName { get; set; } = string.Empty;

    [Required]
    public required string PasswordHash { get; set; } = string.Empty;

    [Required]
    public required byte[] Salt { get; set; } = [];

    public bool IsVerified { get; set; }

    public bool IsEnabled { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
