using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagementService.Application.Services;
using UserManagementService.Domain.Interfaces;
using UserManagementService.Infrastructure.Data;
using UserManagementService.Infrastructure.Repositories;
using UserManagementService.Infrastructure.Services;

namespace UserManagementService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database 
        var postgresConnectionString = configuration.GetConnectionString("DefaultConnection");
        if (postgresConnectionString is null) throw new Exception("Postgres connection string is null");
        services.AddDbContext<AppDbContext>(x =>
        {
            x.UseNpgsql(
                postgresConnectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
                    npgsqlOptions.CommandTimeout(30);
                    npgsqlOptions.MaxBatchSize(100);
                    npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            x.EnableSensitiveDataLogging();
            x.EnableDetailedErrors();
            x.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Services
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserService, UserService>();

        // Healthy Check
        services
            .AddHealthChecks()
            .AddNpgSql(postgresConnectionString);

        return services;
    }
}
