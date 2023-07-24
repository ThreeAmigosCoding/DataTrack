using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IAnalogInputService
{
    Task<AnalogInput> CreateAnalogInput(AnalogInputDto analogInputDto);
}