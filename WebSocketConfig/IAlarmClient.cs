using DataTrack.Dto;

namespace DataTrack.WebSocketConfig;

public interface IAlarmClient
{
    Task ReceiveData(AlarmNotificationDto alarmNotification);
}