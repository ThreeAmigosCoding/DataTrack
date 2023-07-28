using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IDigitalInputService
{
    Task<DigitalInput> CreateDigitalInput(DigitalInputDto digitalInputDto);

    public void StartReadingAll();

}