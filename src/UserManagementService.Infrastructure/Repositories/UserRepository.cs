using Microsoft.EntityFrameworkCore;
using UserManagementService.Domain.Entities;
using UserManagementService.Domain.Interfaces;
using UserManagementService.Infrastructure.Data;

namespace UserManagementService.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }
}
