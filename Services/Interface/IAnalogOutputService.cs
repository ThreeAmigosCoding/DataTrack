using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IAnalogOutputService
{
    public Task<AnalogOutput> ChangeAnalogOutputValue(string IOAddress, double value);
}