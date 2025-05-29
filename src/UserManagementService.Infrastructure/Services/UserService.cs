using MapsterMapper;
using UserManagementService.Application.Dtos.User;
using UserManagementService.Application.Services;
using UserManagementService.Domain.Interfaces;

namespace UserManagementService.Infrastructure.Services;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
{
    public async Task<UserDto?> GetUserByAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        return user is null ? null : mapper.Map<UserDto>(user);
    }
}
