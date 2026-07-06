using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.ViewModels;
using KnowYourCustomerServiceForBank.Server.Repositories;

namespace KnowYourCustomerServiceForBank.Server.Services;

class LoginService(IRepository<User> userRepo, ILogger<LoginService> logger) : ILoginService
{
  private readonly IRepository<User> _userRepo = userRepo;

  private readonly ILogger<LoginService> _logger = logger;

  public async Task<User?> FetchUser(UserLogin model)
  {
    User? user = await _userRepo.GetByConditionAsync(u => u.Email == model.Email);

    return null;
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