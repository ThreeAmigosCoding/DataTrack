using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Model.Utils;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataTrack.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AlarmController : ControllerBase
{
    private readonly IAlarmService _alarmService;
    private readonly IUserService _userService;
    private readonly IAnalogInputService _analogInputService;
    
    public AlarmController(IAlarmService alarmService, IUserService userService, IAnalogInputService analogInputService)
    {
        _alarmService = alarmService;
        _userService = userService;
        _analogInputService = analogInputService;
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost]
    public async Task<ActionResult> CreateAlarm([FromBody] AlarmCreationDto alarmDto)
    {
        try
        {
            await _alarmService.CreateAlarm(alarmDto);
            return Ok(new ResponseMessageDto("Alarm created successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.Message));
        }
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetAllUserAlarms(Guid id)
    {
        try
        {
            return Ok(await _userService.FindAlarmIdsByUser(id));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.Message));
        }
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet("{inputId:guid}")]
    public async Task<ActionResult> GetAllInputAlarms(Guid inputId)
    {
        try
        {
            return Ok(await _analogInputService.GetAlarms(inputId));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.Message));
        }
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAlarm(Guid id)
    {
        try
        {
            await _alarmService.DeleteAlarm(id);
            return Ok(new ResponseMessageDto("Alarm deleted successfully"));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.Message));
        }
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPut]
    public async Task<ActionResult> GetAlarmRecordsByTime([FromBody] DateRangeDto dateRange)
    {
        try
        {
            return Ok(await _alarmService.GetAlarmRecordsByTime(dateRange));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.Message));
        }
    }
    
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpGet]
    public async Task<ActionResult> GetAlarmRecordsByPriority([FromQuery] AlarmPriority priority)
    {
        try
        {
            return Ok(await _alarmService.GetAlarmRecordsByPriority(priority));
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseMessageDto(e.Message));
        }
    }
}