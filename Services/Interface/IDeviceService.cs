using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IDeviceService
{
    public Task<Device> createDevice(DeviceDto deviceDto);
}