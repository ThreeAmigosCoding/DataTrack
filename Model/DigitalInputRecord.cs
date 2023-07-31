using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class DigitalInputRecord : IBaseEntity
{
    public Guid Id { get; set; }
    
    public DateTime RecordedAt { get; set; }
    
    public DigitalInput DigitalInput  { get; set; }
    
    public double Value { get; set; }

    public DigitalInputRecord()
    {
    }

    public DigitalInputRecord(Guid id, DateTime recordedAt, DigitalInput digitalInput, double value)
    {
        Id = id;
        RecordedAt = recordedAt;
        DigitalInput = digitalInput;
        Value = value;
    }
}