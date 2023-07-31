using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class AnalogInputRecordRepository : CrudRepository<AnalogInputRecord>, IAnalogInputRecordRepository
{
    public AnalogInputRecordRepository(DatabaseContext context) : base(context)
    {
    }
}