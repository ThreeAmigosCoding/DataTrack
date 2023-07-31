using DataTrack.Model.Utils;

namespace DataTrack.Dto;

public class AlarmNotificationDto
{
    public DateTime AlarmTime { get; set; }
    public AlarmPriority Priority { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }

    public AlarmNotificationDto()
    {
    }

    public AlarmNotificationDto(DateTime alarmTime, AlarmPriority priority, string title, string message)
    {
        AlarmTime = alarmTime;
        Priority = priority;
        Title = title;
        Message = message;
    }
}