namespace DataTrack.Dto;

public class DeviceDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? IOAddress { get; set; }
    public int LowerBound { get; set; }
    public double? Value { get; set; }
    public int UpperBound { get; set; }
    public bool IsDigital { get; set; }

    public DeviceDto()
    {
    }

    public DeviceDto(Guid id, string name, string ioAddress, int lowerBound, double value, 
        int upperBound, bool isDigital)
    {
        Id = id;
        Name = name;
        IOAddress = ioAddress;
        LowerBound = lowerBound;
        Value = value;
        UpperBound = upperBound;
        IsDigital = isDigital;
    }
}