using DataTrack.Config;
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
        Device device;
        await ScadaConfig.dbSemaphore.WaitAsync();
        try
        {
            device = await _entities.FirstOrDefaultAsync(x => x.Name == name);
            await Task.Delay(1);
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }
        
        return device;
    }

    public async Task<Device?> FindByIoAddress(string ioAddress)
    {
        Device device;
        await ScadaConfig.dbSemaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            device = await _entities.FirstOrDefaultAsync(x => x.IOAddress == ioAddress);
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }

        return device;
    }
}