using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class AnalogInputRecordService : IAnalogInputRecordService
{
    private readonly IAnalogInputRecordRepository _analogInputRecordRepository;

    public AnalogInputRecordService(IAnalogInputRecordRepository analogInputRecordRepository)
    {
        _analogInputRecordRepository = analogInputRecordRepository;
    }

    public async Task<AnalogInputRecord> Create(AnalogInputRecord inputRecord)
    {
        return await _analogInputRecordRepository.Create(inputRecord);
    }

    public async Task<AnalogInputRecord> FindById(Guid id)
    {
        return await _analogInputRecordRepository.Read(id);
    }

    public async Task<List<AnalogInputRecord>> ReadAll()
    {
        return (await _analogInputRecordRepository.ReadAll()).ToList();
    }

    public async Task<List<InputRecordDto>> GetAllAsDto(DateRangeDto dateRange)
    {
        var records = (await ReadAll())
            .Where(r => dateRange.IsDateInRange(r.RecordedAt));
        return records.Select(r => new InputRecordDto(r)).ToList();
    }

    public async Task<List<InputRecordDto>> GetAllByInput(Guid inputId)
    {
        var records = (await ReadAll()).Where(r => r.AnalogInput.Id == inputId)
            .ToList();
        return records.Select(r => new InputRecordDto(r)).OrderByDescending(r => r.RecordedAt)
            .ToList();
    }
}