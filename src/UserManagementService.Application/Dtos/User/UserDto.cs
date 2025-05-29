namespace UserManagementService.Application.Dtos.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public bool IsVerified { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsDeleted { get; set; }
}
