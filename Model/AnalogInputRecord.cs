using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class AnalogInputRecord : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTime RecordedAt { get; set; }
    
    public AnalogInput AnalogInput { get; set; }
    
    public double Value { get; set; }

    public AnalogInputRecord()
    {
    }

    public AnalogInputRecord(Guid id, DateTime recordedAt, AnalogInput analogInput, double value)
    {
        Id = id;
        RecordedAt = recordedAt;
        AnalogInput = analogInput;
        Value = value;
    }
}