using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataTrack.Repositories.Implementation;

public class DeviceRepository : CrudRepository<Device>, IDeviceRepository
{
    public DeviceRepository(DatabaseContext context) : base(context)
    {
    }

    public async Task<Device?> FindByName(string name)
    {
        await Task.Delay(1);
        return await _entities.FirstOrDefaultAsync(x => x.Name == name);
    }
}