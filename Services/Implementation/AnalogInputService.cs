using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;
using DataTrack.WebSocketConfig;
using Microsoft.AspNetCore.SignalR;

namespace DataTrack.Services.Implementation;

public class AnalogInputService : IAnalogInputService
{
    private readonly IAnalogInputRepository _analogInputRepository;
    private readonly IDeviceService _deviceService;
    private readonly IUserService _userService;
    private readonly IAnalogInputRecordService _analogInputRecordService;
    private readonly IHubContext<InputHub, IInputClient> _inputHub;


    public AnalogInputService(IAnalogInputRepository analogInputRepository, IDeviceService deviceService,
        IUserService userService, IHubContext<InputHub, IInputClient> inputHub,
        IAnalogInputRecordService analogInputRecordService)
    {
        _userService = userService;
        _deviceService = deviceService;
        _analogInputRepository = analogInputRepository;
        _analogInputRecordService = analogInputRecordService;
        _inputHub = inputHub;
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

    public async void StartReadingAll()
    {
        List<AnalogInput> analogInputs = (await _analogInputRepository.ReadAll()).ToList();
        foreach (AnalogInput analogInput in analogInputs)
        {
            StartReading(analogInput.Id);
        }
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

                AnalogInputRecord inputRecord = new AnalogInputRecord
                {
                    RecordedAt = DateTime.Now,
                    AnalogInput = analogInput,
                    //TODO: pretvoriti u odgovarajuci unit
                    Value = device.Value
                };
                inputRecord = await _analogInputRecordService.Create(inputRecord);

                //TODO: ovde alarmi 
                if (device.Value > analogInput.HighLimit)
                    Console.WriteLine("High limit reached");
                else if (device.Value < analogInput.LowLimit)
                    Console.WriteLine("Low limit reached");
                
                Console.WriteLine("Tag Scanning -> " + "Device Name: " + device.Name + "; Value: " + device.Value);
                InputRecordDto inputRecordDto = new InputRecordDto(inputRecord, device);
                await _inputHub.Clients.All
                    .ReceiveData(inputRecordDto);

                await Task.Delay(analogInput.ScanTime);
            }
            
        });
    }
}