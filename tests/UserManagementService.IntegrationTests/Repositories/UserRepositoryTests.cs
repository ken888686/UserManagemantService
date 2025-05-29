using UserManagementService.Domain.Interfaces;
using UserManagementService.Infrastructure.Data;
using UserManagementService.Infrastructure.Repositories;

namespace UserManagementService.IntegrationTests.Repositories;

public class UserRepositoryTests(BaseEfRepoTestFixture fixture) : IClassFixture<BaseEfRepoTestFixture>
{
    private readonly IUserRepository _userRepository = new UserRepository(fixture.DbContext);

    [Fact]
    public async Task GetByIdAsync_WithExistingUserId_ReturnsUser()
    {
        // Arrange
        // Act
        var result = await _userRepository.GetByIdAsync(SeedData.UserId1);

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
        var nonExistingUserId = Guid.NewGuid();

        // Act
        var result = await _userRepository.GetByIdAsync(nonExistingUserId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithDeletedUser_ReturnsNull()
    {
        // Arrange
        var deletedUserId = SeedData.UserId2; // Using the second seeded user
        var user = await _userRepository.GetByIdAsync(deletedUserId);
        user!.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        await fixture.DbContext.SaveChangesAsync();

        // Act
        var result = await _userRepository.GetByIdAsync(deletedUserId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.DeletedAt);
        Assert.True(result.IsDeleted);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUserId_IncludesUserRoles()
    {
        // Arrange
        // Act
        var user = await _userRepository.GetByIdAsync(SeedData.UserId1);

        // Assert
        Assert.NotNull(user);
        Assert.NotNull(user.UserRoles);
        Assert.NotEmpty(user.UserRoles);
        Assert.Contains(user.UserRoles, ur => ur.RoleId == SeedData.RoleId1);
    }
}
