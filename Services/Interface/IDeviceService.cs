using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IDeviceService
{
    public Task<Device> CreateDevice(DeviceDto deviceDto);

    public Task<List<Device>>ReadAll();

    public Task<Device> Update(Device device);
    
    public Task<Device> FindByIoAddress(string ioAddress);
}