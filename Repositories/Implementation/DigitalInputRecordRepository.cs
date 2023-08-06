using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class DigitalInputRecordRepository : CrudRepository<DigitalInputRecord>, IDigitalInputRecordRepository
{
    public DigitalInputRecordRepository(DatabaseContext context) : base(context)
    {
    }
}