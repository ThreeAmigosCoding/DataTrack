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
    private readonly IAlarmRecordRepository _alarmRecordRepository;
    
    private readonly IDeviceService _deviceService;
    private readonly IUserService _userService;
    private readonly IAnalogInputRecordService _analogInputRecordService;
    
    private readonly IHubContext<InputHub, IInputClient> _inputHub;
    private readonly IHubContext<AlarmHub, IAlarmClient> _alarmHub;

    public AnalogInputService(
        IAnalogInputRepository analogInputRepository,
        IAlarmRecordRepository alarmRecordRepository,
        IDeviceService deviceService,
        IAnalogInputRecordService analogInputRecordService,
        IUserService userService, 
        IHubContext<InputHub, IInputClient> inputHub,
        IHubContext<AlarmHub, IAlarmClient> alarmHub)
    {
        _analogInputRepository = analogInputRepository;
        _alarmRecordRepository = alarmRecordRepository;
        
        _userService = userService;
        _deviceService = deviceService;
        _analogInputRecordService = analogInputRecordService;
        
        _inputHub = inputHub;
        _alarmHub = alarmHub;
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

                var recordedValue = device.Value;
                if (device.Value > analogInput.HighLimit)
                    recordedValue = analogInput.HighLimit;
                else if (device.Value < analogInput.LowLimit)
                    recordedValue = analogInput.LowLimit;
                
                AnalogInputRecord inputRecord = new AnalogInputRecord
                {
                    RecordedAt = DateTime.Now,
                    AnalogInput = analogInput,
                    // TODO: unit conversion
                    Value = recordedValue
                };
                inputRecord = await _analogInputRecordService.Create(inputRecord);
                
                Alarm(analogInput, device);

                Console.WriteLine("Tag Scanning -> " + "Device Name: " + device.Name + "; Value: " + device.Value);
                InputRecordDto inputRecordDto = new InputRecordDto(inputRecord, device);
                await _inputHub.Clients.All
                    .ReceiveData(inputRecordDto);

                await Task.Delay(analogInput.ScanTime);
            }
            
        });
    }

    // TODO: alarm logs
    private async void Alarm(AnalogInput input, Device device)
    {
        var alarms = input.Alarms;
        foreach (var alarm in alarms)
        {
            if (alarm.Type == AlarmType.LOW && device.Value < alarm.EdgeValue ||
                alarm.Type == AlarmType.HIGH && device.Value > alarm.EdgeValue)
            {
                var messageType = alarm.Type == AlarmType.HIGH ? "' surpassed " : "' dropped below ";

                var title = "Tag '" + input.Id + "'";
                var message = "Device '" + device.Name + messageType + alarm.EdgeValue + " " + alarm.Unit + 
                              " at IO address: " + "'" + device.IOAddress + "'";
                Console.WriteLine("Alarm: Device '" + device.Name + messageType + alarm.EdgeValue + " " + alarm.Unit);
                
                var recordedAt = DateTime.Now;
                await _alarmRecordRepository.Create(new AlarmRecord
                {
                    Alarm = alarm,
                    Value = device.Value,
                    RecordedAt = recordedAt
                });

                await _alarmHub.Clients.All.ReceiveData(new AlarmNotificationDto
                {
                    Priority = alarm.Priority,
                    AlarmTime = recordedAt,
                    Title = title,
                    Message = message
                });
            }
        }
    }
}