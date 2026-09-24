using System.Security.Claims;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.ViewModels;

namespace KnowYourCustomerServiceForBank.Server.Services;

public interface IAuthService
{
  public Task<User?> FetchUser(UserLogin model);
  public ClaimsPrincipal BuildPrincipal(User model);
}