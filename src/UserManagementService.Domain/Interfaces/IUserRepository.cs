using UserManagementService.Domain.Entities;

namespace UserManagementService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
}
