using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class AlarmService : IAlarmService
{
    private readonly IAlarmRepository _alarmRepository;
    private readonly IAnalogInputRepository _analogInputRepository;

    public AlarmService(IAlarmRepository alarmRepository, IAnalogInputRepository analogInputRepository)
    {
        _alarmRepository = alarmRepository;
        _analogInputRepository = analogInputRepository;
    }

    public async Task<Alarm> CreateAlarm(AlarmCreationDto alarmCreationDto)
    {
        var input = await _analogInputRepository.Read(alarmCreationDto.AnalogInputId);
        if (input == null)
            throw new Exception("Input does not exist.");
        
        var alarm = new Alarm
        {
            Type = alarmCreationDto.Type,
            Priority = alarmCreationDto.Priority,
            EdgeValue = alarmCreationDto.EdgeValue,
            Unit = alarmCreationDto.Unit,
            AnalogInput = input
        };

        var newAlarm = await _alarmRepository.Create(alarm);
        
        return newAlarm;
    }
}