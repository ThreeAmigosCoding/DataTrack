using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class RealTimeUnit : IBaseEntity
{
    public Guid Id { get; set; }
    public String Address { get; set; }
    public double Value { get; set; }

    public RealTimeUnit()
    {
    }

    public RealTimeUnit(Guid id, string address, double value)
    {
        Id = id;
        Address = address;
        Value = value;
    }
}