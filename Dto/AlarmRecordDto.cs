using DataTrack.Model;

namespace DataTrack.Dto;

public class AlarmRecordDto
{
    public Guid Id { get; set; }
    public AlarmDto AlarmDto { get; set; }
    public double Value { get; set; }
    public DateTime RecordedAt { get; set; }

    public AlarmRecordDto()
    {
    }

    public AlarmRecordDto(Guid id, AlarmDto alarmDto, double value, DateTime recordedAt)
    {
        Id = id;
        AlarmDto = alarmDto;
        Value = value;
        RecordedAt = recordedAt;
    }

    public AlarmRecordDto(AlarmRecord alarmRecord)
    {
        Id = alarmRecord.Id;
        AlarmDto = new AlarmDto
        {
            AnalogInputId = alarmRecord.Alarm.AnalogInput.Id,
            EdgeValue = alarmRecord.Alarm.EdgeValue,
            Id = alarmRecord.Alarm.Id,
            Priority = alarmRecord.Alarm.Priority,
            Type = alarmRecord.Alarm.Type,
            Unit = alarmRecord.Alarm.Unit
        };
        Value = alarmRecord.Value;
        RecordedAt = alarmRecord.RecordedAt;
    }
}