using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataTrack.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AlarmController : ControllerBase
{
    private readonly IAlarmService _alarmService;
    
    public AlarmController(IAlarmService alarmService)
    {
        _alarmService = alarmService;
    }

    // [Authorize(Policy = IdentityData.AdminUserPolicyName)]
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
}