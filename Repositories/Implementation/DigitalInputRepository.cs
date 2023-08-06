using DataTrack.Config;
using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataTrack.Repositories.Implementation;

public class DigitalInputRepository : CrudRepository<DigitalInput>, IDigitalInputRepository
{
    public DigitalInputRepository(DatabaseContext context) : base(context)
    {
    }

    public async Task<DigitalInput> FindById(Guid id)
    {
        await ScadaConfig.dbSemaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await _entities
                .Include(u => u.Users)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }
    }
}