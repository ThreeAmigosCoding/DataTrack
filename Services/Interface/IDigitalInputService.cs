using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IDigitalInputService
{
    Task<DigitalInput> CreateDigitalInput(DigitalInputDto digitalInputDto);

    public void StartReadingAll();
    
    public Task<List<InputRecordDto>> GetAllByUser(Guid id);

    public Task<DigitalInput> SwitchDigitalInputState(Guid id);
    
    public Task<List<Guid>> GetAllInputIds();
    
}