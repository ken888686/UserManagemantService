using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using UserManagementService.Infrastructure.Data;

namespace UserManagementService.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await SeedData.InitializeAsync(dbContext);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = builder.Build();
        host.Start();
        return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        // Replace connection string with testContainers
        builder.UseSetting("ConnectionStrings:DefaultConnection", _postgresContainer.GetConnectionString());

        // JWT settings
        builder.UseSetting("JwtSettings:Key", "soIG4qu3HUGohHAspYT/CpXA93FYVz9s7JaXZwdEaZs=");
        builder.UseSetting("JwtSettings:Issuer", "FunctionalTests");
        builder.UseSetting("JwtSettings:Audience", "https://localhost:3001/");
        builder.UseSetting("JwtSettings:ExpiryInDays", "30");
    }
}
