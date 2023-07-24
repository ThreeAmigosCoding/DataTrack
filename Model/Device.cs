using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class Device : IBaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IOAddress { get; set; }
    public int LowerBound { get; set; }
    public double Value { get; set; }
    public int UpperBound { get; set; }
    public bool IsDigital { get; set; }
    public string Driver { get; set; }

    public Device()
    {
    }

    public Device(Guid id, string name, string ioAddress, int lowerBound, double value, int upperBound, 
        bool isDigital, string driver)
    {
        Id = id;
        Name = name;
        IOAddress = ioAddress;
        LowerBound = lowerBound;
        Value = value;
        UpperBound = upperBound;
        IsDigital = isDigital;
        Driver = driver;
    }
}