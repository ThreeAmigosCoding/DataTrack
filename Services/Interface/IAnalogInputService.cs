using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IAnalogInputService
{
    Task<AnalogInput> CreateAnalogInput(AnalogInputDto analogInputDto);
    
    public void StartReadingAll();

    public Task<List<InputRecordDto>> GetAllByUser(Guid id);

    public Task<AnalogInput> SwitchAnalogInputState(Guid id);
    
    public Task<List<AlarmDto>> GetAlarms(Guid inputId);

    public Task<List<Guid>> GetAllInputIds();

    public Task DeleteInput(string ioAddress);

}