using DataTrack.Model;

namespace DataTrack.Repositories.Interface;

public interface IUserRepository : ICrudRepository<User>
{
    public Task<User?> FindByEmail(string email);
    
    public Task<List<User>> FindByAdminId(Guid adminId);

    public Task<User> FindById(Guid id);
}