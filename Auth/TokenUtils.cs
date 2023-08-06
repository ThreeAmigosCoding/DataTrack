using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataTrack.Model;
using Microsoft.IdentityModel.Tokens;

namespace DataTrack.Auth;

public static class TokenUtils
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(24);
    
    public static string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Secrets.Secrets.Key);

        List<Claim> claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("email", user.Email),
            new("admin", user.Admin.ToString())
        };

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.Add(TokenLifetime),
            Issuer = "dataTrack-server",
            Audience = "dataTrack-client",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}