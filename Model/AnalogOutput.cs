using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class AnalogOutput : IBaseEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string IOAddress { get; set; }
    public double LowLimit { get; set; }
    public double HighLimit { get; set; }
    public string Unit { get; set; }
    
    public double InitialValue { get; set; }

    public AnalogOutput()
    {
    }

    public AnalogOutput(Guid id, string description, string ioAddress, double lowLimit, double highLimit, string unit,
        double initialValue)
    {
        Id = id;
        Description = description;
        IOAddress = ioAddress;
        LowLimit = lowLimit;
        HighLimit = highLimit;
        Unit = unit;
        InitialValue = initialValue;
    }
}