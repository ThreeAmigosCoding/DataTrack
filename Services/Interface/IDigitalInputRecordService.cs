using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IDigitalInputRecordService
{
    public Task<DigitalInputRecord> Create(DigitalInputRecord digitalInputRecord);
    
    public Task<DigitalInputRecord> FindById(Guid id);
    
    public Task<List<DigitalInputRecord>> ReadAll();
    
    public Task<List<InputRecordDto>> GetAllAsDto(DateRangeDto dateRange);
    public Task<List<InputRecordDto>> GetAllByInput(Guid inputId);
}