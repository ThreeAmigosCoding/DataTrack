using DataTrack.Dto;
using Microsoft.AspNetCore.SignalR;

namespace DataTrack.WebSocketConfig;

public class AlarmHub : Hub<IAlarmClient>
{
    public AlarmHub()
    {
    }

    public async Task SendDataToClient(AlarmNotificationDto alarmNotification)
    {
        await Clients.All.ReceiveData(alarmNotification);
    }
}