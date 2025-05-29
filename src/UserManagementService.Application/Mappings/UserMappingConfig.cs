using Mapster;
using UserManagementService.Application.Dtos.User;
using UserManagementService.Domain.Entities;

namespace UserManagementService.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>();
    }
}
