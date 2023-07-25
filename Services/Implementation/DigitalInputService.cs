using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class DigitalInputService : IDigitalInputService
{
    private readonly IDigitalInputRepository _digitalInputRepository;
    private readonly IDeviceService _deviceService;
    private readonly IUserService _userService;


    public DigitalInputService(IDigitalInputRepository digitalInputRepository, IDeviceService deviceService, IUserService userService)
    {
        _digitalInputRepository = digitalInputRepository;
        _deviceService = deviceService;
        _userService = userService;
    }

    public async Task<DigitalInput> CreateDigitalInput(DigitalInputDto digitalInputDto)
    {
        Device device = await _deviceService.CreateDevice(digitalInputDto.Device);
        List<User> users = await _userService.FindUsersByAdmin(digitalInputDto.CreatedBy);
        DigitalInput digitalInput = new DigitalInput
        {
            Description = digitalInputDto.Description,
            IOAddress = device.IOAddress,
            ScanTime = digitalInputDto.ScanTime,
            ScanOn = true,
            Users = users
        };

        DigitalInput createdDigitalInput = await _digitalInputRepository.Create(digitalInput);
        StartReading(createdDigitalInput.Id);
        return createdDigitalInput;
    }

    public void StartReading(Guid inputId)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                DigitalInput digitalInput = await _digitalInputRepository.Read(inputId);
                
                if (!digitalInput.ScanOn) continue;
                Device device = await _deviceService.FindByIoAddress(digitalInput.IOAddress);
                Console.WriteLine("Device Name: " + device.Name + "; Value: " + device.Value);
                await Task.Delay(digitalInput.ScanTime);
                
            }
        });
    }
}