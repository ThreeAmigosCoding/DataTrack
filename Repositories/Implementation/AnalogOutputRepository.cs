using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class AnalogOutputRepository: CrudRepository<AnalogOutput>, IAnalogOutputRepository
{
    public AnalogOutputRepository(DatabaseContext context) : base(context)
    {
    }
}