using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class DigitalOutput : IBaseEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string IOAddress { get; set; }
    public double InitialValue { get; set; }
    public bool Deleted { get; set; }

    public DigitalOutput()
    {
    }

    public DigitalOutput(Guid id, string description, string ioAddress, double initialValue, bool deleted)
    {
        Id = id;
        Description = description;
        IOAddress = ioAddress;
        InitialValue = initialValue;
        Deleted = deleted;
    }
}