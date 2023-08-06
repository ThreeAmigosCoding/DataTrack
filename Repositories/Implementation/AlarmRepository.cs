using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class AlarmRepository : CrudRepository<Alarm>, IAlarmRepository
{
    public AlarmRepository(DatabaseContext context) : base(context)
    {
    }
}