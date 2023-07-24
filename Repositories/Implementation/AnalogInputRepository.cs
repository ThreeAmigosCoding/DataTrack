using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class AnalogInputRepository : CrudRepository<AnalogInput>, IAnalogInputRepository
{
    public AnalogInputRepository(DatabaseContext context) : base(context)
    {
    }
}