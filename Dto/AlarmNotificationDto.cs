using DataTrack.Model.Utils;

namespace DataTrack.Dto;

public class AlarmNotificationDto
{
    public Guid AlarmId { get; set; }
    public AlarmPriority Priority { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }

    public AlarmNotificationDto()
    {
    }

    public AlarmNotificationDto(Guid alarmId, AlarmPriority priority, string title, string message)
    {
        AlarmId = alarmId;
        Priority = priority;
        Title = title;
        Message = message;
    }
}