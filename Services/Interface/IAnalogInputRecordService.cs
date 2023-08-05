using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IAnalogInputRecordService
{
    public Task<AnalogInputRecord> Create(AnalogInputRecord inputRecord);

    public Task<AnalogInputRecord> FindById(Guid id);
    
    public Task<List<AnalogInputRecord>> ReadAll();

    public Task<List<InputRecordDto>> GetAllAsDto(DateRangeDto dateRange);
    
    public Task<List<InputRecordDto>> GetAllByInput(Guid inputId);
}