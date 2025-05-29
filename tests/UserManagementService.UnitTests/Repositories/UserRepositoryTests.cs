using Microsoft.EntityFrameworkCore;
using UserManagementService.Domain.Entities;
using UserManagementService.Domain.Interfaces;
using UserManagementService.Infrastructure.Data;
using UserManagementService.Infrastructure.Repositories;

namespace UserManagementService.UnitTests.Repositories;

public class UserRepositoryTests
{
    private readonly IUserRepository _repository;
    private readonly Guid _userId1 = Guid.NewGuid();
    private readonly Guid _userId2 = Guid.NewGuid();
    private readonly Guid _userRoleId1 = Guid.NewGuid();
    private readonly Guid _userRoleId2 = Guid.NewGuid();

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var appDbContext = new AppDbContext(options);
        appDbContext.Database.EnsureCreated();
        appDbContext.Users.AddRange(
            new User
            {
                Id = _userId1,
                Email = "ken888686@gmail.com",
                UserName = "ken888686",
                PasswordHash = "password",
                Salt = [1, 2, 3, 4],
                IsVerified = true,
                IsEnabled = true
            },
            new User
            {
                Id = _userId2,
                Email = "ken666868@gmail.com",
                UserName = "ken666868",
                PasswordHash = "password",
                Salt = [1, 2, 3, 4],
                IsVerified = true,
                IsEnabled = true
            }
        );
        appDbContext.Roles.AddRange(
            new Role
            {
                Name = "Admin"
            },
            new Role
            {
                Name = "User"
            }
        );
        appDbContext.UserRoles.AddRange(
            new UserRole
            {
                UserId = _userId1,
                RoleId = _userRoleId1
            },
            new UserRole
            {
                UserId = _userId2,
                RoleId = _userRoleId2
            }
        );
        appDbContext.SaveChanges();

        _repository = new UserRepository(appDbContext);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var expectedUser = new User
        {
            Id = _userId1,
            Email = "ken888686@gmail.com",
            UserName = "ken888686",
            PasswordHash = "password",
            Salt = [1, 2, 3, 4],
            IsVerified = true,
            IsEnabled = true
        };

        // Act
        var result = await _repository.GetByIdAsync(_userId1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_userId1, result.Id);
        Assert.Equal(expectedUser.Email, result.Email);
        Assert.Equal(expectedUser.UserName, result.UserName);
        Assert.Equal(expectedUser.IsEnabled, result.IsEnabled);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }
}
