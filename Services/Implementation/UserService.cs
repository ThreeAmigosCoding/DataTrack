using System.Security.Authentication;
using DataTrack.Auth;
using DataTrack.Dto;
using DataTrack.Model;
using DataTrack.Repositories.Interface;
using DataTrack.Services.Interface;

namespace DataTrack.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> RegisterUser(UserDto userDto, string registeredBy)
    {
        if (await _userRepository.FindByEmail(userDto.Email) != null)
        {
            throw new Exception("User already registered");
        }

        User user = new User();
        user.Email = userDto.Email;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        user.RegisteredBy = await _userRepository.FindByEmail(registeredBy);
        user.Admin = false;

        return await _userRepository.Create(user);
    }
    
    public async Task<string> Login(LoginDto loginDto)
    {
        User user = await _userRepository.FindByEmail(loginDto.Email) ?? 
                    throw new AuthenticationException("Wrong email or password.");

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            throw new AuthenticationException("Wrong email or password.");
        
        return TokenUtils.GenerateToken(user);
    }

}