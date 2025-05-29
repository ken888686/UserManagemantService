using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using UserManagementService.Infrastructure.Data;

namespace UserManagementService.IntegrationTests;

public class BaseEfRepoTestFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _postgresContainer;
    public AppDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _postgresContainer = new PostgreSqlBuilder().WithCleanUp(true).Build();
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
        await _postgresContainer!.StopAsync();
        await _postgresContainer!.DisposeAsync();
    }
}
