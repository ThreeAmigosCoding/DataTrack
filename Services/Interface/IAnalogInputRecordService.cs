using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IAnalogInputRecordService
{
    public Task<AnalogInputRecord> Create(AnalogInputRecord inputRecord);

    public Task<AnalogInputRecord> FindById(Guid id);
    
    public Task<List<AnalogInputRecord>> ReadAll();
}