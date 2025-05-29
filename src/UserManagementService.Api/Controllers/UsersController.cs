using Microsoft.AspNetCore.Mvc;
using UserManagementService.Application.Services;

namespace UserManagementService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var userDto = await userService.GetUserByAsync(id);
        return userDto is null ? NoContent() : Ok(userDto);
    }
}
