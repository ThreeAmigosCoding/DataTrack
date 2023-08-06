using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataTrack.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OutputController : ControllerBase
{
    private readonly IAnalogOutputService _analogOutputService;
    private readonly IDigitalOutputService _digitalOutputService;

    public OutputController(
        IAnalogOutputService analogOutputService, 
        IDigitalOutputService digitalOutputService
        )
    {
        _analogOutputService = analogOutputService;
        _digitalOutputService = digitalOutputService;
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut]
    public async Task<ActionResult> ChangeAnalogOutputValue([FromQuery] string ioAddress, [FromQuery] double value)
    {
        try
        {
            await _analogOutputService.ChangeAnalogOutputValue(ioAddress, value);
            return Ok(new ResponseMessageDto("Analog output with IO Address: " + ioAddress + " changed successfully."));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.StackTrace));
        }
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut]
    public async Task<ActionResult> ChangeDigitalOutputValue([FromQuery] string ioAddress, [FromQuery] double value)
    {
        try
        {
            await _digitalOutputService.ChangeDigitalOutputValue(ioAddress, value);
            return Ok(new ResponseMessageDto("Digital output with IO Address: " + ioAddress + " changed successfully."));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.StackTrace));
        }
    }
}