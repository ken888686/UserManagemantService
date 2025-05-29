namespace UserManagementService.FunctionalTests.Endpoints;

public class HealthCheckTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task HealthCheck_ReturnsHealthy_ForPostgres()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/health");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Healthy", content);
    }
}
