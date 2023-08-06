namespace DataTrack.Dto;

public class AnalogInputDto
{
    public DeviceDto Device { get; set; }
    
    public string Description { get; set; }
    
    public int ScanTime { get; set; }
    
    public double LowLimit { get; set; }
    
    public double HighLimit { get; set; }
    
    public string Unit { get; set; }
    
    public Guid CreatedBy { get; set; }

    public AnalogInputDto()
    {
    }

    public AnalogInputDto(DeviceDto device, string description, int scanTime, double lowLimit, double highLimit, 
        string unit, Guid createdBy)
    {
        Device = device;
        Description = description;
        ScanTime = scanTime;
        LowLimit = lowLimit;
        HighLimit = highLimit;
        Unit = unit;
        CreatedBy = createdBy;
    }
}