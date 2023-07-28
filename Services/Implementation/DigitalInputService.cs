using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;
using DataTrack.WebSocketConfig;
using Microsoft.AspNetCore.SignalR;

namespace DataTrack.Services.Implementation;

public class DigitalInputService : IDigitalInputService
{
    private readonly IDigitalInputRepository _digitalInputRepository;
    private readonly IDeviceService _deviceService;
    private readonly IUserService _userService;
    private readonly IDigitalInputRecordService _digitalInputRecordService;
    private readonly IHubContext<InputHub, IInputClient> _inputHub;



    public DigitalInputService(IDigitalInputRepository digitalInputRepository, IDeviceService deviceService, 
        IUserService userService, IHubContext<InputHub, IInputClient> inputHub, 
        IDigitalInputRecordService digitalInputRecordService)
    {
        _digitalInputRepository = digitalInputRepository;
        _deviceService = deviceService;
        _userService = userService;
        _digitalInputRecordService = digitalInputRecordService;
        _inputHub = inputHub;
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
    
    public async void StartReadingAll()
    {
        List<DigitalInput> digitalInputs = (await _digitalInputRepository.ReadAll()).ToList();
        foreach (DigitalInput digitalInput in digitalInputs)
        {
            StartReading(digitalInput.Id);
        }
    }

    public async Task<List<InputRecordDto>> GetAllByUser(Guid id)
    {
        var user = await _userService.FindById(id);
        if (user == null) throw new Exception("User doesn't exist.");
        var inputRecords = new List<InputRecordDto>();

        foreach (var input in user.DigitalInputs)
        {
            var device = await _deviceService.FindByIoAddress(input.IOAddress);
            var digitalInputRecord = new DigitalInputRecord
            {
                Id = new Guid(),
                RecordedAt = DateTime.Now,
                Value = device.Value,
                DigitalInput = input
            };
            inputRecords.Add(new InputRecordDto(digitalInputRecord, device));
        }

        return inputRecords;
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

                DigitalInputRecord digitalInputRecord = new DigitalInputRecord
                {
                    DigitalInput = digitalInput,
                    RecordedAt = DateTime.Now,
                    //TODO: pretvoriti u odgovarajuci unit
                    Value = device.Value
                };
                digitalInputRecord = await _digitalInputRecordService.Create(digitalInputRecord);
                
                Console.WriteLine("Tag Scanning -> " + "Device Name: " + device.Name + "; Value: " + device.Value);
                InputRecordDto inputRecordDto = new InputRecordDto(digitalInputRecord, device);
                await _inputHub.Clients.All
                    .ReceiveData(inputRecordDto);

                await Task.Delay(digitalInput.ScanTime);
                
            }
        });
    }
}