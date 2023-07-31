using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IAnalogInputService
{
    Task<AnalogInput> CreateAnalogInput(AnalogInputDto analogInputDto);
    
    public void StartReadingAll();

    public Task<List<InputRecordDto>> GetAllByUser(Guid id);
}