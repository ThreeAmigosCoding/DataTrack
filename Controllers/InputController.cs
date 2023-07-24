using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataTrack.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InputController : ControllerBase
{
    private readonly IAnalogInputService _analogInputService;
    private readonly IDigitalInputService _digitalInputService;

    public InputController(IAnalogInputService analogInputService, IDigitalInputService digitalInputService)
    {
        _analogInputService = analogInputService;
        _digitalInputService = digitalInputService;
    }

    //[Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost]
    public async Task<ActionResult> CreateAnalogInput([FromBody] AnalogInputDto analogInputDto)
    {
        AnalogInput analogInput = await _analogInputService.CreateAnalogInput(analogInputDto);
        return Ok(new ResponseMessageDto("Analog Input created successfully"));
    }
    
    // public async Task<ActionResult> CreateDigitalInput([FromBody] DigitalInputDto digitalInputDto)
    // {
    //     return Ok();
    // }
}