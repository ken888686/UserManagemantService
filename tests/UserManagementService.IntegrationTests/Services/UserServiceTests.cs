using Mapster;
using MapsterMapper;
using UserManagementService.Infrastructure.Data;
using UserManagementService.Infrastructure.Repositories;
using UserManagementService.Infrastructure.Services;

namespace UserManagementService.IntegrationTests.Services;

public class UserServiceTests(BaseEfRepoTestFixture fixture) : IClassFixture<BaseEfRepoTestFixture>
{
    [Fact]
    public async Task GetUserAsync_WithExistingUserId_ReturnsUserDto()
    {
        // Arrange
        var repository = new UserRepository(fixture.DbContext);
        var config = new TypeAdapterConfig();
        var mapper = new Mapper(config);
        var userService = new UserService(repository, mapper);

        // Act
        var result = await userService.GetUserAsync(SeedData.UserId1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SeedData.UserId1, result.Id);
        Assert.Equal(SeedData.UserName1, result.UserName);
        Assert.Equal($"{SeedData.UserName1}@gmail.com", result.Email);
        Assert.True(result.IsEnabled);
    }

    [Fact]
    public async Task GetUserAsync_WithNonExistingUserId_ReturnsEmptyUserDto()
    {
        // Arrange
        var repository = new UserRepository(fixture.DbContext);
        var config = new TypeAdapterConfig();
        var mapper = new Mapper(config);
        var userService = new UserService(repository, mapper);
        var nonExistingUserId = Guid.NewGuid();

        // Act
        var result = await userService.GetUserAsync(nonExistingUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Guid.Empty, result.Id);
        Assert.Null(result.UserName);
        Assert.Null(result.Email);
        Assert.False(result.IsEnabled);
    }
}
