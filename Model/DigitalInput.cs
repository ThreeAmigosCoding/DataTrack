using DataTrack.Model.Utils;

namespace DataTrack.Model;

public class DigitalInput : IBaseEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Driver { get; set; }
    public string IOAddress { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
    public List<User> Users { get; set; }

    public DigitalInput()
    {
    }

    public DigitalInput(Guid id, string description, string driver, string ioAddress, int scanTime,
        bool scanOn, List<User> users)
    {
        Id = id;
        Description = description;
        Driver = driver;
        IOAddress = ioAddress;
        ScanTime = scanTime;
        ScanOn = scanOn;
        Users = users;
    }
}