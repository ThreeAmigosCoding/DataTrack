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
        await Task.Delay(1);
        return await _entities.FirstOrDefaultAsync(x => x.Email == email);
    }
}