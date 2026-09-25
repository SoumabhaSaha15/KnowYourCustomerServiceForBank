using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using KnowYourCustomerServiceForBank.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.ViewModels;
using KnowYourCustomerServiceForBank.Server.Annotations;

namespace KnowYourCustomerServiceForBank.Server.Services;


[ServiceLifetime(ServiceLifetime.Scoped)]
public class AuthService(AppDbContext context, ILogger<AuthService> logger, IConfiguration config) : IAuthService
{
  public static List<Claim> CreateUserClaims(User user) =>
    [
      new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
      new(JwtRegisteredClaimNames.Name, user.FullName),
      new(JwtRegisteredClaimNames.Email, user.Email),
      new("role", user.UserRole.ToString())
    ];
  private readonly ILogger<AuthService> _logger = logger;
  private readonly AppDbContext _context = context;
  private static readonly PasswordHasher<User> _hasher = new();

  public async Task<User?> FetchUser(UserLogin model)
  {
    User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
    if (user == null) return null;
    var result = _hasher.VerifyHashedPassword(user, user.Password, model.Password);
    if (result == PasswordVerificationResult.Failed) return null;
    else return user;
  }

  public ClaimsPrincipal BuildPrincipal(User user)
  {
    List<Claim> claims = CreateUserClaims(user);
    ClaimsIdentity identity = new(
      claims: claims,
      authenticationType: CookieAuthenticationDefaults.AuthenticationScheme,
      nameType: JwtRegisteredClaimNames.Name,
      roleType: "role"
    );
    return new ClaimsPrincipal(identity);
  }

  public string GenerateAccessToken(User user)
  {
    var tokenSection = config.GetSection("Jwt");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSection["Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: CreateUserClaims(user),
        expires: DateTime.UtcNow.AddMinutes(15),
        signingCredentials: creds
      );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}