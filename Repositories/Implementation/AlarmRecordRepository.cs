using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class AlarmRecordRepository : CrudRepository<AlarmRecord>, IAlarmRecordRepository
{
    public AlarmRecordRepository(DatabaseContext context) : base(context)
    {
    }
}