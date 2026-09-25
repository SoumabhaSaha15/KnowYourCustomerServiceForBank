using KnowYourCustomerServiceForBank.Server.Models;

namespace KnowYourCustomerServiceForBank.Server.Services;

public interface ITokenService
{
  string GenerateAccessToken(User user);
}