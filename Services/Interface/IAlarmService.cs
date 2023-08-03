using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IAlarmService
{
    public Task<Alarm> CreateAlarm(AlarmCreationDto alarmCreationDto);
    public Task<Alarm> DeleteAlarm(Guid id);
    public Task<List<AlarmRecordDto>> GetAlarmRecordsByTime(DateRangeDto dateRange);
}