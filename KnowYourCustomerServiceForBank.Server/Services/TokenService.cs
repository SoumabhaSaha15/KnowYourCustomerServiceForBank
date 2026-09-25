using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using KnowYourCustomerServiceForBank.Server.Models;

namespace KnowYourCustomerServiceForBank.Server.Services;

public class TokenService(IConfiguration config) : ITokenService
{
  public string GenerateAccessToken(User user)
  {
    var jwtSection = config.GetSection("Jwt");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    List<Claim> claims =
      [
        new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new(ClaimTypes.Role, user.UserRole.ToString()),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.Name, user.FullName)
      ];

    var token = new JwtSecurityToken(
        issuer: jwtSection["Issuer"],
        audience: jwtSection["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(15),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}