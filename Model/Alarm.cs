using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class Alarm : IBaseEntity
{
    public Guid Id { get; set; }
    public AlarmType Type { get; set; }
    public AlarmPriority Priority { get; set; }
    public double EdgeValue { get; set; }
    public string Unit { get; set; }
    public AnalogInput AnalogInput { get; set; }
    public bool Deleted { get; set; }

    public Alarm()
    {
    }

    public Alarm(Guid id, AlarmType type, AlarmPriority priority, double edgeValue, 
        string unit, AnalogInput analogInput, bool deleted)
    {
        Id = id;
        Type = type;
        Priority = priority;
        EdgeValue = edgeValue;
        Unit = unit;
        AnalogInput = analogInput;
        Deleted = deleted;
    }
}