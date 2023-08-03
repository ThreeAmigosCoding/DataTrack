using DataTrack.Model.Utils;

namespace DataTrack.Dto;

public class AlarmDto
{
    public Guid Id { get; set; }
    public AlarmType Type { get; set; }
    public AlarmPriority Priority { get; set; }
    public double EdgeValue { get; set; }
    public string Unit { get; set; }
    public Guid AnalogInputId { get; set; }

    public AlarmDto()
    {
    }

    public AlarmDto(Guid id, AlarmType type, AlarmPriority priority, double edgeValue, string unit, Guid analogInputId)
    {
        Id = id;
        Type = type;
        Priority = priority;
        EdgeValue = edgeValue;
        Unit = unit;
        AnalogInputId = analogInputId;
    }
}