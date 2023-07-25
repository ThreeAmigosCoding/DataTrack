using DataTrack.Dto;
using DataTrack.Model;

namespace DataTrack.Services.Interface;

public interface IUserService
{
    public Task<User> RegisterUser(UserDto user, string registeredBy);

    public Task<string> Login(LoginDto loginDto);

    public Task<List<User>> FindUsersByAdmin(Guid adminId);
}