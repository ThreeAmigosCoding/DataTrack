using DataTrack.Config;
using DataTrack.DataBase;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataTrack.Repositories.Implementation;

public class UserRepository : CrudRepository<User>, IUserRepository
{
    public UserRepository(DatabaseContext context) : base(context)
    {
    }

    public async Task<User?> FindByEmail(string email)
    {
        User user;
        await ScadaConfig.dbSemaphore.WaitAsync();
        try
        {
            await Task.Delay(1);
            user = await _entities.FirstOrDefaultAsync(x => x.Email == email);
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }
        
        return user;
        
    }

    public async Task<List<User>> FindByAdminId(Guid adminId)
    {
        List<User> users = new List<User>();
        await ScadaConfig.dbSemaphore.WaitAsync();
        try
        {
            await Task.Delay(1);
            users =  await _entities
                .Where(x => x.Id == adminId || x.RegisteredBy.Id == adminId)
                .ToListAsync();
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }

        return users;

    }

    public async Task<User> FindById(Guid id)
    {
        await ScadaConfig.dbSemaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await _entities
                .Include(e => e.AnalogInputs).ThenInclude(e => e.Alarms)
                .Include(e => e.DigitalInputs)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        finally
        {
            ScadaConfig.dbSemaphore.Release();
        }
        
    }
}