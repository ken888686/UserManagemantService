using UserManagementService.Application.Dtos.User;

namespace UserManagementService.Application.Services;

public interface IUserService
{
    Task<UserDto?> GetUserByAsync(Guid id);
}
