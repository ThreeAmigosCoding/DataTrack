using DataTrack.Config;
using DataTrack.Model;

namespace DataTrack.Repositories.Interface;

public interface IAnalogInputRepository : ICrudRepository<AnalogInput>
{
    public Task<AnalogInput> FindById(Guid id);
}