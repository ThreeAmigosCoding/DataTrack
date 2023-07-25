using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class AnalogInputService : IAnalogInputService
{
    private readonly IAnalogInputRepository _analogInputRepository;
    private readonly IDeviceService _deviceService;
    private readonly IUserService _userService;

    public AnalogInputService(IAnalogInputRepository analogInputRepository, IDeviceService deviceService,
        IUserService userService)
    {
        _userService = userService;
        _deviceService = deviceService;
        _analogInputRepository = analogInputRepository;
    }

    public async Task<AnalogInput> CreateAnalogInput(AnalogInputDto analogInputDto)
    {
        Device device = await _deviceService.CreateDevice(analogInputDto.Device);
        List<User> users = await _userService.FindUsersByAdmin(analogInputDto.CreatedBy);
        AnalogInput analogInput = new AnalogInput
        {
            Description = analogInputDto.Description,
            ScanTime = analogInputDto.ScanTime,
            LowLimit = analogInputDto.LowLimit,
            HighLimit = analogInputDto.HighLimit,
            Unit = analogInputDto.Unit,
            IOAddress = device.IOAddress,
            ScanOn = true,
            Users = users
        };
        
        AnalogInput createdAnalogInput = await _analogInputRepository.Create(analogInput);
        StartReading(createdAnalogInput.Id);

        return createdAnalogInput;
    }

    public void StartReading(Guid inputId)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                AnalogInput analogInput = await _analogInputRepository.Read(inputId);
                
                if (!analogInput.ScanOn) continue;
                Device device = await _deviceService.FindByIoAddress(analogInput.IOAddress);

                //TODO: ovde alarmi 
                if (device.Value > analogInput.HighLimit)
                    Console.WriteLine("High limit reached");
                else if (device.Value < analogInput.LowLimit)
                    Console.WriteLine("Low limit reached");
                
                Console.WriteLine("Device Name: " + device.Name + "Value: " + device.Value);
                
                await Task.Delay(analogInput.ScanTime);
            }
            
        });
    }
}