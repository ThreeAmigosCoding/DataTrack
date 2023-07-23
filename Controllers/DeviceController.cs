using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataTrack.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost]
    public async Task<ActionResult> CreateDevice([FromBody] DeviceDto deviceDto)
    {
        try
        {
            await _deviceService.createDevice(deviceDto);
            return Ok(new ResponseMessageDto("Device created successfully!"));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.Message));
        }
    }
}