using DataTrack.Dto;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DataTrack.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<ActionResult> register([FromBody] UserDto userDto, [FromQuery] string registeredBy)
    {
        await _userService.RegisterUser(userDto, registeredBy);
        return Ok("Registration successful");
    }
}