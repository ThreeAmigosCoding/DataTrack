using DataTrack.Model.Utils;

namespace DataTrack.Dto;

public class AlarmCreationDto
{
    public AlarmType Type { get; set; }
    public AlarmPriority Priority { get; set; }
    public double EdgeValue { get; set; }
    public string Unit { get; set; }
    public Guid AnalogInputId { get; set; }

    public AlarmCreationDto()
    {
    }

    public AlarmCreationDto(AlarmType type, AlarmPriority priority, double edgeValue, string unit, Guid analogInputId)
    {
        Type = type;
        Priority = priority;
        EdgeValue = edgeValue;
        Unit = unit;
        AnalogInputId = analogInputId;
    }
}