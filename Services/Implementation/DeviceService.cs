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
    
    public async Task<Device> CreateDevice(DeviceDto deviceDto)
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
            Driver = deviceDto.Driver
        };
        
        return await _deviceRepository.Create(device);
    }

    public async Task<List<Device>> ReadAll()
    {
        return (await _deviceRepository.ReadAll()).ToList();
    }

    public Task<Device> Update(Device device)
    {
        return _deviceRepository.Update(device);
    }

    public async Task<Device> FindByIoAddress(string ioAddress)
    {
        return await _deviceRepository.FindByIoAddress(ioAddress) ?? 
               throw new Exception("There is no device with the specified address.");
    }
}