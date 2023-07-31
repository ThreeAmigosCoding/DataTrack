using DataTrack.Config;
using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Model.Utils;
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
    private readonly IAlarmRecordRepository _alarmRecordRepository;


    public AnalogInputService(IAnalogInputRepository analogInputRepository, IDeviceService deviceService,
        IUserService userService, IHubContext<InputHub, IInputClient> inputHub,
        IAnalogInputRecordService analogInputRecordService,
        IAlarmRecordRepository alarmRecordRepository)
    {
        _userService = userService;
        _deviceService = deviceService;
        _analogInputRepository = analogInputRepository;
        _analogInputRecordService = analogInputRecordService;
        _inputHub = inputHub;
        _alarmRecordRepository = alarmRecordRepository;
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

    public async Task<List<InputRecordDto>> GetAllByUser(Guid id)
    {
        var user = await _userService.FindById(id);
        if (user == null) throw new Exception("User doesn't exist.");
        var inputRecords = new List<InputRecordDto>();

        foreach (var input in user.AnalogInputs)
        {
            var device = await _deviceService.FindByIoAddress(input.IOAddress);
            var analogInputRecord = new AnalogInputRecord
            {
                Id = new Guid(),
                RecordedAt = DateTime.Now,
                Value = device.Value,
                AnalogInput = input
            };
            inputRecords.Add(new InputRecordDto(analogInputRecord, device));
        }

        return inputRecords;
    }

    private void StartReading(Guid inputId)
    {
        Task.Run(async () =>
        {
            
            while (true)
            {
                AnalogInput analogInput = await _analogInputRepository.FindById(inputId);
                
                if (!analogInput.ScanOn) continue;
                Device device = await _deviceService.FindByIoAddress(analogInput.IOAddress);

                AnalogInputRecord inputRecord = new AnalogInputRecord
                {
                    RecordedAt = DateTime.Now,
                    AnalogInput = analogInput,
                    // TODO: pretvoriti u odgovarajuci unit
                    Value = device.Value
                };
                inputRecord = await _analogInputRecordService.Create(inputRecord);
                
                Alarm(analogInput, device);

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

    private void Alarm(AnalogInput input, Device device)
    {
        var alarms = input.Alarms;
        foreach (var alarm in alarms)
        {
            if (alarm.Type == AlarmType.LOW && device.Value < alarm.EdgeValue)
            {
                Console.WriteLine("Alarm: Tag on device '" + device.Name + 
                                  "' dropped below " + alarm.EdgeValue + " " + alarm.Unit);
                _alarmRecordRepository.Create(new AlarmRecord
                {
                    Alarm = alarm,
                    Value = device.Value,
                    RecordedAt = DateTime.Now
                });
            }
            else if (alarm.Type == AlarmType.HIGH && device.Value > alarm.EdgeValue)
            {
                Console.WriteLine("Alarm: Tag on device '" + device.Name + 
                                  "' surpassed " + alarm.EdgeValue + " " + alarm.Unit);
                _alarmRecordRepository.Create(new AlarmRecord
                {
                    Alarm = alarm,
                    Value = device.Value,
                    RecordedAt = DateTime.Now
                });
            }
        }
    }
}