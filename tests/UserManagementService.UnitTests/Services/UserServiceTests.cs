using MapsterMapper;
using Moq;
using UserManagementService.Application.Dtos.User;
using UserManagementService.Domain.Entities;
using UserManagementService.Domain.Interfaces;
using UserManagementService.Infrastructure.Services;

namespace UserManagementService.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetUserAsync_WhenUserExists_ReturnsMappedUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "john.doe@example.com",
            UserName = "johndoe",
            PasswordHash = "hashedPassword",
            Salt = [1, 2, 3, 4],
            IsVerified = true,
            IsEnabled = true
        };

        var expectedUserDto = new UserDto
        {
            Id = userId,
            Email = "john.doe@example.com",
            UserName = "johndoe",
            IsVerified = true,
            IsEnabled = true
        };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
        _mockMapper.Setup(mapper => mapper.Map<UserDto>(user)).Returns(expectedUserDto);

        // Act
        var result = await _userService.GetUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUserDto.Id, result.Id);
        Assert.Equal(expectedUserDto.Email, result.Email);
        Assert.Equal(expectedUserDto.UserName, result.UserName);
        Assert.Equal(expectedUserDto.IsVerified, result.IsVerified);
        Assert.Equal(expectedUserDto.IsEnabled, result.IsEnabled);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<UserDto>(user), Times.Once);
    }

    [Fact]
    public async Task GetUserAsync_WhenUserDoesNotExist_ReturnsEmptyUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Guid.Empty, result.Id);
        Assert.Null(result.Email);
        Assert.Null(result.UserName);
        Assert.False(result.IsVerified);
        Assert.False(result.IsEnabled);

        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }
}
