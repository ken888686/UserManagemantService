using System.Net;
using System.Text.Json;
using UserManagementService.Application.Dtos.User;
using UserManagementService.Infrastructure.Data;

namespace UserManagementService.FunctionalTests.Endpoints;

public class UsersControllerTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAsync_WhenUserExists_ReturnsOkWithUser()
    {
        // Arrange
        var userId = SeedData.UserId1; // Use a known test user ID

        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
    }

    [Fact]
    public async Task GetAsync_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/users/{nonExistentUserId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetAsync_WithInvalidGuid_ReturnsBadRequest()
    {
        // Arrange
        const string invalidGuid = "not-a-guid";

        // Act
        var response = await _client.GetAsync($"/api/users/{invalidGuid}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
