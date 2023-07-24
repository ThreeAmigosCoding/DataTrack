namespace DataTrack.Dto;

public class DigitalInputDto
{
    public DeviceDto Device { get; set; }
    
    public string Description { get; set; }
    
    public int ScanTime { get; set; }
    
    public Guid CreatedBy { get; set; }

    public DigitalInputDto()
    {
    }

    public DigitalInputDto(DeviceDto device, string description, int scanTime, Guid createdBy)
    {
        Device = device;
        Description = description;
        ScanTime = scanTime;
        CreatedBy = createdBy;
    }
}