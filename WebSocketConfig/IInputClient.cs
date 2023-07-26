using DataTrack.Dto;

namespace DataTrack.WebSocketConfig;

public interface IInputClient
{
    Task ReceiveAnalogData(ResponseMessageDto data);
    Task ReceiveDigitalData(ResponseMessageDto data);
}