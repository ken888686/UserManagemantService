using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementService.Domain.Entities;

namespace UserManagementService.Infrastructure.Data.Config;

public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(x => new { x.UserId, x.RoleId });
        builder.ConfigureTimestamps();

        builder.HasOne(x => x.Role).WithMany(r => r.UserRoles);
        builder.HasOne(x => x.User).WithMany(u => u.UserRoles);
    }
}
