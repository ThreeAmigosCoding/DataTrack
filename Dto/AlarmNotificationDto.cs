using DataTrack.Model.Utils;

namespace DataTrack.Dto;

public class AlarmNotificationDto
{
    public AlarmPriority Priority { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }

    public AlarmNotificationDto()
    {
    }

    public AlarmNotificationDto(AlarmPriority priority, string title, string message)
    {
        Priority = priority;
        Title = title;
        Message = message;
    }
}