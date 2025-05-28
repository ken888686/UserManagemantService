namespace UserManagementService.Domain.Interfaces;

public interface IPasswordService
{
    string Hash(string password, out byte[] salt);
    bool VerifyPassword(string password, string storedHash, byte[] storedSalt);
}
