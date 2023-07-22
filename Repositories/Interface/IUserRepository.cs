using DataTrack.Model;

namespace DataTrack.Repositories.Interface;

public interface IUserRepository : ICrudRepository<User>
{
    public Task<User?> FindByEmail(string email);
}