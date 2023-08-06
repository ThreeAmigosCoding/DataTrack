using DataTrack.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DataTrack.WebSocketConfig;

public class InputHub : Hub<IInputClient>
{
    public InputHub()
    {
    }
    
    public async Task SendDataToClients(InputRecordDto data)
    {
        await Clients.All.ReceiveData(data);
    }
    
}