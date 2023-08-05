using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IDigitalOutputService
{
    public Task<DigitalOutput> ChangeDigitalOutputValue(string IOAddress, double value);

}