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
}