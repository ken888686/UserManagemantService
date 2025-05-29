using UserManagementService.Domain.Entities;
using UserManagementService.Infrastructure.Services;

namespace UserManagementService.Infrastructure.Data;

public static class SeedData
{
    private const string UserPassword = "test";
    public const string UserName1 = "ken888686";
    public const string UserName2 = "ken666868";
    public const string RoleName1 = "admin";
    public const string RoleName2 = "user";

    public static Guid UserId1 = Guid.Parse("b48b972f-28bf-4693-b7b4-714303ee725a");
    public static Guid UserId2 = Guid.Parse("7c1067a3-3602-468d-a085-ab8d486ffa25");
    public static Guid RoleId1 = Guid.Parse("7f549cfe-e364-4a5c-be4a-db687dba0f43");
    public static Guid RoleId2 = Guid.Parse("306f444d-8619-492f-911c-91af69a660f5");

    public static async Task InitializeAsync(AppDbContext dbContext)
    {
        await PopulateTestDataAsync(dbContext);
    }

    private static async Task PopulateTestDataAsync(AppDbContext dbContext)
    {
        var passwordService = new PasswordService();

        // User
        var passwordHash = passwordService.Hash(UserPassword, out var salt);
        dbContext.Users.AddRange(
            new User
            {
                Id = UserId1,
                Email = $"{UserName1}@gmail.com",
                UserName = UserName1,
                PasswordHash = passwordHash,
                Salt = salt,
                IsEnabled = true,
                IsVerified = true,
                IsDeleted = false
            },
            new User
            {
                Id = UserId2,
                Email = $"{UserName2}@gmail.com",
                UserName = UserName2,
                PasswordHash = passwordHash,
                Salt = salt,
                IsEnabled = true,
                IsVerified = true,
                IsDeleted = false
            });

        // Role
        dbContext.Roles.AddRange(
            new Role
            {
                Id = RoleId1,
                Name = RoleName1
            },
            new Role
            {
                Id = RoleId2,
                Name = RoleName2
            }
        );

        // UserRole
        dbContext.UserRoles.AddRange(
            new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = UserId1,
                RoleId = RoleId1
            },
            new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = UserId2,
                RoleId = RoleId2
            });

        await dbContext.SaveChangesAsync();
    }
}
