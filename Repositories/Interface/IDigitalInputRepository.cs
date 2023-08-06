using DataTrack.Model;

namespace DataTrack.Repositories.Interface;

public interface IDigitalInputRepository : ICrudRepository<DigitalInput>
{
    public Task<DigitalInput> FindById(Guid id);

}