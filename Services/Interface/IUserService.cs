using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IUserService
{
    public Task<User> RegisterUser(UserDto user, string registeredBy);

    public Task<User> Login(string email, string password);
}