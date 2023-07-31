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

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost]
    public async Task<ActionResult> CreateAnalogInput([FromBody] AnalogInputDto analogInputDto)
    {
        AnalogInput analogInput = await _analogInputService.CreateAnalogInput(analogInputDto);
        return Ok(new ResponseMessageDto("Analog Input created successfully."));
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost]
    public async Task<ActionResult> CreateDigitalInput([FromBody] DigitalInputDto digitalInputDto)
    {
        DigitalInput digitalInput = await _digitalInputService.CreateDigitalInput(digitalInputDto);
        return Ok(new ResponseMessageDto("Digital Input created successfully."));
    }

    //[Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetAllUserInputs(Guid id)
    {
        try
        {
            return Ok((await _analogInputService.GetAllByUser(id)).Concat(await _digitalInputService.GetAllByUser(id)));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.StackTrace));
        }
    }
}