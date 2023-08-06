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
    private readonly IDigitalOutputRepository _digitalOutputRepository;
    private readonly IDeviceRepository _deviceRepository;
    
    private readonly IDeviceService _deviceService;
    private readonly IUserService _userService;
    private readonly IDigitalInputRecordService _digitalInputRecordService;
    
    private readonly IHubContext<InputHub, IInputClient> _inputHub;



    public DigitalInputService(
        IDigitalInputRepository digitalInputRepository,
        IDigitalOutputRepository digitalOutputRepository,
        IDeviceService deviceService, 
        IUserService userService,
        IDigitalInputRecordService digitalInputRecordService,
        IHubContext<InputHub, IInputClient> inputHub,
        IDeviceRepository deviceRepository
        )
    {
        _digitalInputRepository = digitalInputRepository;
        _digitalOutputRepository = digitalOutputRepository;
        _deviceRepository = deviceRepository;
        
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
        DigitalOutput digitalOutput = new DigitalOutput
        {
            Description = digitalInputDto.Description,
            InitialValue = 0,
            IOAddress = device.IOAddress
        };

        DigitalInput createdDigitalInput = await _digitalInputRepository.Create(digitalInput);
        await _digitalOutputRepository.Create(digitalOutput);
        StartReading(createdDigitalInput.Id);
        return createdDigitalInput;
    }
    
    public async void StartReadingAll()
    {
        List<DigitalInput> digitalInputs = (await _digitalInputRepository.ReadAll()).Where(d => !d.Deleted).ToList();
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
            if (input.Deleted) continue;
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

    public async Task<DigitalInput> SwitchDigitalInputState(Guid id)
    {
        DigitalInput digitalInput = await _digitalInputRepository.FindById(id);
        digitalInput.ScanOn = !digitalInput.ScanOn;
        return await _digitalInputRepository.Update(digitalInput);
    }

    public async Task<List<Guid>> GetAllInputIds()
    {
        return (await _digitalInputRepository.ReadAll()).Select(i => i.Id).ToList();
    }

    public async Task DeleteInput(string ioAddress)
    {
        DigitalInput input = (await _digitalInputRepository.ReadAll())
            .FirstOrDefault(i => i.IOAddress == ioAddress);
        input.Deleted = true;

        Device device = await _deviceRepository.FindByIoAddress(ioAddress);
        device.Deleted = true;
        
        DigitalOutput output = (await _digitalOutputRepository.ReadAll())
            .FirstOrDefault(o => o.IOAddress == ioAddress);
        output.Deleted = true;
        
        await _digitalInputRepository.Update(input);
        await _deviceRepository.Update(device);
        await _digitalOutputRepository.Update(output);
    }

    public void StartReading(Guid inputId)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                DigitalInput digitalInput = await _digitalInputRepository.Read(inputId);
                
                if (!digitalInput.ScanOn || digitalInput.Deleted) continue;
                Device device = await _deviceService.FindByIoAddress(digitalInput.IOAddress);

                DigitalInputRecord digitalInputRecord = new DigitalInputRecord
                {
                    DigitalInput = digitalInput,
                    RecordedAt = DateTime.Now,
                    //TODO: pretvoriti u odgovarajuci unit
                    Value = device.Value
                };
                digitalInputRecord = await _digitalInputRecordService.Create(digitalInputRecord);
                
                InputRecordDto inputRecordDto = new InputRecordDto(digitalInputRecord, device);
                await _inputHub.Clients.All
                    .ReceiveData(inputRecordDto);

                await Task.Delay(digitalInput.ScanTime);
                
            }
        });
    }
}