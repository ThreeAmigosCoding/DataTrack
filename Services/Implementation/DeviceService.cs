using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }
    
    public async Task<Device> createDevice(DeviceDto deviceDto)
    {
        if (await _deviceRepository.FindByName(deviceDto.Name) != null)
            throw new Exception("Device with this name already exists!");
            
        var device = new Device
        {
            Name = deviceDto.Name,
            IOAddress = Guid.NewGuid().ToString(),
            IsDigital = deviceDto.IsDigital,
            LowerBound = deviceDto.LowerBound,
            UpperBound = deviceDto.UpperBound,
        };
        
        return await _deviceRepository.Create(device);
    }
}