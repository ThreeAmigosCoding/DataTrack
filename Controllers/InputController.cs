using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DataTrack.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InputController : ControllerBase
{
    private readonly IAnalogInputService _analogInputService;
    private readonly IDigitalInputService _digitalInputService;

    private readonly IAnalogInputRecordService _analogInputRecordService;
    private readonly IDigitalInputRecordService _digitalInputRecordService;

    public InputController(IAnalogInputService analogInputService, IDigitalInputService digitalInputService, 
        IAnalogInputRecordService analogInputRecordService, IDigitalInputRecordService digitalInputRecordService)
    {
        _analogInputService = analogInputService;
        _digitalInputService = digitalInputService;
        _analogInputRecordService = analogInputRecordService;
        _digitalInputRecordService = digitalInputRecordService;
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

    [Authorize]
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
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> SwitchAnalogInputState(Guid id)
    {
        await _analogInputService.SwitchAnalogInputState(id);
        return Ok(new ResponseMessageDto("Analog Input: " + id + " state changed successfully."));
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> SwitchDigitalInputState(Guid id)
    {
        await _digitalInputService.SwitchDigitalInputState(id);
        return Ok(new ResponseMessageDto("Digital Input: " + id + " state changed successfully."));
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut]
    public async Task<ActionResult> GetAllInputRecords([FromBody] DateRangeDto dateRange)
    {
        var analogInputRecordsDto = await _analogInputRecordService.GetAllAsDto(dateRange);
        var digitalInputRecordsDto = await _digitalInputRecordService.GetAllAsDto(dateRange);

        return Ok(analogInputRecordsDto.Concat(digitalInputRecordsDto).OrderByDescending(r => r.RecordedAt));
    }
    
    //[Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet("{inputId:guid}")]
    public async Task<ActionResult> GetAllInputRecordsByInput(Guid inputId)
    {
        var recordDtos = await _digitalInputRecordService.GetAllByInput(inputId);
        if (recordDtos.IsNullOrEmpty())
            recordDtos = await _analogInputRecordService.GetAllByInput(inputId);
        return Ok(recordDtos);
    }
}