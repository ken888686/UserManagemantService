using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementService.Domain.Entities;

namespace UserManagementService.Infrastructure.Data.Config;

public static class BaseEntityConfig
{
    public static void ConfigureTimestamps<T>(this EntityTypeBuilder<T> builder) where T : BaseEntity
    {
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
            .ValueGeneratedOnUpdate()
            .IsRequired();

        builder.Property(x => x.DeletedAt);

        builder.Property(x => x.IsDeleted).HasDefaultValue(false);
    }
}
