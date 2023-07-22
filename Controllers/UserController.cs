using System.Security.Authentication;
using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] UserDto userDto, [FromQuery] string registeredBy)
    {
        await _userService.RegisterUser(userDto, registeredBy);
        return Ok("Registration successful");
    }

    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            return Ok(new MyToken(await _userService.Login(loginDto)));
        }
        catch (AuthenticationException e)
        {
            return BadRequest(new ErrorDto(e.Message));
        }
    }
    
}