using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using UserManagementService.Application.Mappings;

namespace UserManagementService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Mapster
        var config = new TypeAdapterConfig();
        var userMappingAssembly = typeof(UserMappingConfig).Assembly;
        config.Scan(userMappingAssembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
