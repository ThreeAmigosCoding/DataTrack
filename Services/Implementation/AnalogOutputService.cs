using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class AnalogOutputService: IAnalogOutputService
{
    private readonly IAnalogOutputRepository _analogOutputRepository;
    private readonly IDeviceRepository _deviceRepository;

    public AnalogOutputService(IAnalogOutputRepository analogOutputRepository, IDeviceRepository deviceRepository)
    {
        _analogOutputRepository = analogOutputRepository;
        _deviceRepository = deviceRepository;
    }

    public async Task<AnalogOutput> ChangeAnalogOutputValue(string IOAddress, double value)
    {
        var device = (await _deviceRepository.ReadAll()).FirstOrDefault(d => d.IOAddress == IOAddress);
        device.Value = value;
        if (device.Value > device.UpperBound)
            device.Value = device.UpperBound;
        if (device.Value < device.LowerBound)
            device.Value = device.LowerBound;
        await _deviceRepository.Update(device);
        
        var analogOutput =
            (await _analogOutputRepository.ReadAll()).FirstOrDefault(o => o.IOAddress == IOAddress);
        analogOutput.InitialValue = value;
        return await _analogOutputRepository.Update(analogOutput);
    }
}