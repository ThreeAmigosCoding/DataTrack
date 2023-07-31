using DataTrack.Dto;

namespace DataTrack.WebSocketConfig;

public interface IInputClient
{
    Task ReceiveData(InputRecordDto data);
}