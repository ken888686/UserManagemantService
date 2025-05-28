using UserManagementService.Infrastructure.Data;
using UserManagementService.Infrastructure.Services;

namespace UserManagementService.IntegrationTests.Data;

public class UserRepository(BaseEfRepoTestFixture fixture) : IClassFixture<BaseEfRepoTestFixture>
{
    [Fact]
    public async Task GetUser_WithUserId_ReturnsUser()
    {
        // Arrange
        var repository = new Infrastructure.Repositories.UserRepository(fixture.DbContext);
        const string testPassword = "test";
        var passwordService = new PasswordService();

        // Act
        var user = await repository.GetByIdAsync(SeedData.UserId1);
        var verifyPassword = passwordService.VerifyPassword(testPassword, user!.PasswordHash, user!.Salt);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(SeedData.UserId1, user.Id);
        Assert.Equal(SeedData.UserName1, user.UserName);
        Assert.True(verifyPassword);
    }
}
