using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;

namespace DataTrack.Repositories.Implementation;

public class DigitalOutputRepository: CrudRepository<DigitalOutput>, IDigitalOutputRepository
{
    public DigitalOutputRepository(DatabaseContext context) : base(context)
    {
    }
}