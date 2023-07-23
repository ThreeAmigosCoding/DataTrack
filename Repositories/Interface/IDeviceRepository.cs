using DataTrack.Model;

namespace DataTrack.Repositories.Interface;

public interface IDeviceRepository : ICrudRepository<Device>
{
    public Task<Device?> FindByName(string name);
}