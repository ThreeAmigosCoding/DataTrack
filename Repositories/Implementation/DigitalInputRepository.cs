using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class DigitalInputRepository : CrudRepository<DigitalInput>, IDigitalInputRepository
{
    public DigitalInputRepository(DatabaseContext context) : base(context)
    {
    }
}