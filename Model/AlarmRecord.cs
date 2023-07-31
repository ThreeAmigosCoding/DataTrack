using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class AlarmRecord : IBaseEntity
{
    public Guid Id { get; set; }
    public Alarm Alarm { get; set; }
    public double Value { get; set; }
    public DateTime RecordedAt { get; set; }

    public AlarmRecord()
    {
    }

    public AlarmRecord(Guid id, Alarm alarm, double value, DateTime recordedAt)
    {
        Id = id;
        Alarm = alarm;
        Value = value;
        RecordedAt = recordedAt;
    }
}