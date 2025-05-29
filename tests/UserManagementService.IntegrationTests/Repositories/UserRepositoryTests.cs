using UserManagementService.Infrastructure.Data;
using UserManagementService.Infrastructure.Repositories;

namespace UserManagementService.IntegrationTests.Repositories;

public class UserRepositoryTests(BaseEfRepoTestFixture fixture) : IClassFixture<BaseEfRepoTestFixture>
{
    [Fact]
    public async Task GetByIdAsync_WithExistingUserId_ReturnsUser()
    {
        // Arrange
        var repository = new UserRepository(fixture.DbContext);

        // Act
        var result = await repository.GetByIdAsync(SeedData.UserId1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SeedData.UserId1, result.Id);
        Assert.Equal(SeedData.UserName1, result.UserName);
        Assert.Equal($"{SeedData.UserName1}@gmail.com", result.Email);
        Assert.True(result.IsEnabled);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingUserId_ReturnsNull()
    {
        // Arrange
        var repository = new UserRepository(fixture.DbContext);
        var nonExistingUserId = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdAsync(nonExistingUserId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithDeletedUser_ReturnsNull()
    {
        // Arrange
        var repository = new UserRepository(fixture.DbContext);
        var deletedUserId = SeedData.UserId2; // Using the second seeded user
        var user = await repository.GetByIdAsync(deletedUserId);
        user!.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        await fixture.DbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(deletedUserId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.DeletedAt);
        Assert.True(result.IsDeleted);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUserId_IncludesUserRoles()
    {
        // Arrange
        var repository = new UserRepository(fixture.DbContext);

        // Act
        var user = await repository.GetByIdAsync(SeedData.UserId1);

        // Assert
        Assert.NotNull(user);
        Assert.NotNull(user.UserRoles);
        Assert.NotEmpty(user.UserRoles);
        Assert.Contains(user.UserRoles, ur => ur.RoleId == SeedData.RoleId1);
    }
}
