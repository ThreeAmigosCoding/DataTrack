using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Model.Utils;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class AlarmService : IAlarmService
{
    private readonly IAlarmRepository _alarmRepository;
    private readonly IAnalogInputRepository _analogInputRepository;
    private readonly IAlarmRecordRepository _alarmRecordRepository;

    public AlarmService(IAlarmRepository alarmRepository, IAnalogInputRepository analogInputRepository, 
        IAlarmRecordRepository alarmRecordRepository)
    {
        _alarmRepository = alarmRepository;
        _analogInputRepository = analogInputRepository;
        _alarmRecordRepository = alarmRecordRepository;
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

    public async Task<Alarm> DeleteAlarm(Guid id)
    {
        var alarm = await _alarmRepository.Read(id);
        alarm.Deleted = true;
        return await _alarmRepository.Update(alarm);
    }

    public async Task<List<AlarmRecordDto>> GetAlarmRecordsByTime(DateRangeDto dateRange)
    {
        var records = (await _alarmRecordRepository.ReadAll())
            .Where(a => dateRange.IsDateInRange(a.RecordedAt))
            .OrderByDescending(a => a.Alarm.Priority).ThenByDescending(a => a.RecordedAt);
        return records.Select(r => new AlarmRecordDto(r)).ToList();
    }

    public async Task<List<AlarmRecordDto>> GetAlarmRecordsByPriority(AlarmPriority priority)
    {
        var records = (await _alarmRecordRepository.ReadAll())
            .Where(a => a.Alarm.Priority == priority).OrderByDescending(a => a.RecordedAt);
        
        return records.Select(r => new AlarmRecordDto(r)).ToList();
        
    }
}