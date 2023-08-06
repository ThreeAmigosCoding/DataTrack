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
    private readonly IAnalogOutputRepository _analogOutputRepository;
    private readonly IAlarmRecordRepository _alarmRecordRepository;
    private readonly IDeviceRepository _deviceRepository;
    
    private readonly IDeviceService _deviceService;
    private readonly IUserService _userService;
    private readonly IAnalogInputRecordService _analogInputRecordService;
    
    private readonly IHubContext<InputHub, IInputClient> _inputHub;
    private readonly IHubContext<AlarmHub, IAlarmClient> _alarmHub;

    public AnalogInputService(
        IAnalogInputRepository analogInputRepository,
        IAnalogOutputRepository analogOutputRepository,
        IAlarmRecordRepository alarmRecordRepository,
        IDeviceService deviceService,
        IAnalogInputRecordService analogInputRecordService,
        IUserService userService, 
        IHubContext<InputHub, IInputClient> inputHub,
        IHubContext<AlarmHub, IAlarmClient> alarmHub,
        IDeviceRepository deviceRepository)
        
    {
        _analogInputRepository = analogInputRepository;
        _analogOutputRepository = analogOutputRepository;
        _alarmRecordRepository = alarmRecordRepository;
        _deviceRepository = deviceRepository;
        
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
        AnalogOutput analogOutput = new AnalogOutput
        {
            Description = analogInput.Description,
            HighLimit = analogInput.HighLimit,
            LowLimit = analogInput.LowLimit,
            IOAddress = analogInput.IOAddress,
            Unit = analogInput.Unit,
            InitialValue = (analogInput.LowLimit + analogInput.HighLimit) / 2
        };
        
        AnalogInput createdAnalogInput = await _analogInputRepository.Create(analogInput);
        await _analogOutputRepository.Create(analogOutput);
        StartReading(createdAnalogInput.Id);

        return createdAnalogInput;
    }

    public async void StartReadingAll()
    {
        List<AnalogInput> analogInputs = (await _analogInputRepository.ReadAll()).Where(a => !a.Deleted).ToList();
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
            if (input.Deleted) continue;
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

    public async Task<AnalogInput> SwitchAnalogInputState(Guid id)
    {
        AnalogInput analogInput = await _analogInputRepository.FindById(id);
        analogInput.ScanOn = !analogInput.ScanOn;
        return await _analogInputRepository.Update(analogInput);
    }

    public async Task<List<AlarmDto>> GetAlarms(Guid inputId)
    {
        var input = await _analogInputRepository.FindById(inputId);
        List<AlarmDto> alarms = new List<AlarmDto>();
        foreach (var alarm in input.Alarms.Where(a => a.Deleted == false))
        {
            alarms.Add(new AlarmDto
            { 
                Id = alarm.Id,
                Type = alarm.Type, 
                Priority = alarm.Priority,
                EdgeValue = alarm.EdgeValue,
                Unit = alarm.Unit,
                AnalogInputId = alarm.AnalogInput.Id
            });
        }
        return alarms;
    }

    public async Task<List<Guid>> GetAllInputIds()
    {
        return (await _analogInputRepository.ReadAll()).Select(i => i.Id).ToList();
    }

    public async Task DeleteInput(string ioAddress)
    {
        AnalogInput input = (await _analogInputRepository.ReadAll())
            .FirstOrDefault(i => i.IOAddress == ioAddress);
        input.Deleted = true;

        Device device = await _deviceRepository.FindByIoAddress(ioAddress);
        device.Deleted = true;
        
        AnalogOutput output = (await _analogOutputRepository.ReadAll())
            .FirstOrDefault(o => o.IOAddress == ioAddress);
        output.Deleted = true;
        
        await _analogInputRepository.Update(input);
        await _deviceRepository.Update(device);
        await _analogOutputRepository.Update(output);
        
    } 

    private void StartReading(Guid inputId)
    {
        Task.Run(async () =>
        {
            
            while (true)
            {
                AnalogInput analogInput = await _analogInputRepository.FindById(inputId);
                
                if (!analogInput.ScanOn || analogInput.Deleted) continue;
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

                InputRecordDto inputRecordDto = new InputRecordDto(inputRecord, device);
                await _inputHub.Clients.All
                    .ReceiveData(inputRecordDto);

                await Task.Delay(analogInput.ScanTime);
            }
            
        });
    }

    private async void Alarm(AnalogInput input, Device device)
    {
        var alarms = input.Alarms.Where(a => a.Deleted == false);
        foreach (var alarm in alarms)
        {
            if (alarm.Type == AlarmType.LOWER && device.Value < alarm.EdgeValue ||
                alarm.Type == AlarmType.HIGHER && device.Value > alarm.EdgeValue)
            {
                var messageType = alarm.Type == AlarmType.HIGHER ? "' surpassed " : "' dropped below ";

                var title = "Tag '" + input.Id + "'";
                var message = "Device '" + device.Name + messageType + alarm.EdgeValue + " " + alarm.Unit + 
                              "\nat IO address: " + "'" + device.IOAddress + "'.";
                Console.WriteLine("Alarm: Device '" + device.Name + messageType + alarm.EdgeValue + " " + alarm.Unit);
                
                var recordedAt = DateTime.Now;
                var alarmRecord = await _alarmRecordRepository.Create(new AlarmRecord
                {
                    Alarm = alarm,
                    Value = device.Value,
                    RecordedAt = recordedAt
                });

                LogAlarm(alarmRecord, input);
                
                await _alarmHub.Clients.All.ReceiveData(new AlarmNotificationDto
                {
                    AlarmId = alarm.Id,
                    Priority = alarm.Priority,
                    Title = title,
                    Message = message + "\n" + recordedAt.ToString("HH:mm:ss dd/MM/yyyy")
                });
            }
        }
    }

    private async void LogAlarm(AlarmRecord alarmRecord, AnalogInput input)
    {
        await ScadaConfig.logSemaphore.WaitAsync();

        try
        {
            await using var outputFile = new StreamWriter("alarm.log", true);
            await outputFile.WriteAsync(
                "Alarm '" + alarmRecord.Alarm.Id + 
                "' for input '" + input.Id + 
                "' triggered for value of " + alarmRecord.Value + " " + alarmRecord.Alarm.Unit + 
                "at " + alarmRecord.RecordedAt.ToString("dd/MM/yyyy HH:mm:ss")
                + "\n");
        }
        finally
        {
            ScadaConfig.logSemaphore.Release();
        }
    }
    
}