using DataTrack.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DataTrack.WebSocketConfig;

public class InputHub : Hub<IInputClient>
{
    public InputHub()
    {
    }
    
    public async Task SendAnalogDataToClients(ResponseMessageDto data)
    {
        await Clients.All.ReceiveAnalogData(data);
    }

    public async Task SendDigitalDataToClients(ResponseMessageDto data)
    {
        await Clients.All.ReceiveDigitalData(data);
    }
}