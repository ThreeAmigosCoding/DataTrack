using System;

namespace DataTrack.Dto
{
    public class DateRangeDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateRangeDto()
        {
        }

        public DateRangeDto(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool IsDateInRange(DateTime date)
        {
            return date >= StartTime && date <= EndTime;
        }
    }
}