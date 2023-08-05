using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class DigitalOutputService: IDigitalOutputService
{
    private readonly IDigitalOutputRepository _digitalOutputRepository;
    private readonly IDeviceRepository _deviceRepository;

    public DigitalOutputService(IDigitalOutputRepository digitalOutputRepository, IDeviceRepository deviceRepository)
    {
        _digitalOutputRepository = digitalOutputRepository;
        _deviceRepository = deviceRepository;
    }

    public async Task<DigitalOutput> ChangeDigitalOutputValue(string IOAddress, double value)
    {
        var device = (await _deviceRepository.ReadAll()).FirstOrDefault(d => d.IOAddress == IOAddress);
        device.Value = value;
        await _deviceRepository.Update(device);
        
        var digitalOutput = 
            (await _digitalOutputRepository.ReadAll()).FirstOrDefault(o => o.IOAddress == IOAddress);
        digitalOutput.InitialValue = value;
        return await _digitalOutputRepository.Update(digitalOutput);
    }
}