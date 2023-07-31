using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class DigitalInputRecordService : IDigitalInputRecordService
{
    private readonly IDigitalInputRecordRepository _digitalInputRecordRepository;

    public DigitalInputRecordService(IDigitalInputRecordRepository digitalInputRecordRepository)
    {
        _digitalInputRecordRepository = digitalInputRecordRepository;
    }

    public async Task<DigitalInputRecord> Create(DigitalInputRecord digitalInputRecord)
    {
        return await _digitalInputRecordRepository.Create(digitalInputRecord);
    }

    public async Task<DigitalInputRecord> FindById(Guid id)
    {
        return await _digitalInputRecordRepository.Read(id);
    }

    public async Task<List<DigitalInputRecord>> ReadAll()
    {
        return (await _digitalInputRecordRepository.ReadAll()).ToList();
    }
}