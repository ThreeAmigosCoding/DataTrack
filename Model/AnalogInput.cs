using DataTrack.Model.Utils;

namespace DataTrack.Model;


public class AnalogInput : IBaseEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string IOAddress { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
    public double LowLimit { get; set; }
    public double HighLimit { get; set; }
    public string Unit { get; set; }
    public List<Alarm> Alarms { get; set; }
    public List<User> Users { get; set; }
    public bool Deleted { get; set; }

    public AnalogInput() { }

    public AnalogInput(Guid id, string description, string ioAddress, int scanTime, bool scanOn, 
        double lowLimit, double highLimit, string unit, List<Alarm> alarms, List<User> users, bool deleted)
    {
        Id = id;
        Description = description;
        IOAddress = ioAddress;
        ScanTime = scanTime;
        ScanOn = scanOn;
        LowLimit = lowLimit;
        HighLimit = highLimit;
        Unit = unit;
        Alarms = alarms;
        Users = users;
        Deleted = deleted;
    }
}