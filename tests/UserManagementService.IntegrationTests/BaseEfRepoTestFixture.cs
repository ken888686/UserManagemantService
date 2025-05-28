using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using UserManagementService.Infrastructure.Data;

namespace UserManagementService.IntegrationTests;

public class BaseEfRepoTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder().Build();
    public required AppDbContext DbContext;

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        var connectionString = _postgresContainer.GetConnectionString();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        DbContext = new AppDbContext(options);
        await DbContext.Database.EnsureCreatedAsync();
        await SeedData.InitializeAsync(DbContext);
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _postgresContainer.StopAsync();
        await _postgresContainer.DisposeAsync();
    }
}
