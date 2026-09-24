using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KnowYourCustomerServiceForBank.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.ViewModels;
using KnowYourCustomerServiceForBank.Server.Annotations;

namespace KnowYourCustomerServiceForBank.Server.Services;


[ServiceLifetime(ServiceLifetime.Scoped)]
public class AuthService(AppDbContext context, ILogger<AuthService> logger) : IAuthService
{
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
    List<Claim> claims = [
        new (ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new (ClaimTypes.Role, user.UserRole.ToString()),
        new (ClaimTypes.Email, user.Email),
        new (ClaimTypes.Name, user.FullName),
    ];
    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    return new ClaimsPrincipal(identity);
  }
}