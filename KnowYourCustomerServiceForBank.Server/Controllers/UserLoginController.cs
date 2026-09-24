using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Services;
using KnowYourCustomerServiceForBank.Server.ViewModels;
namespace KnowYourCustomerServiceForBank.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserAuthenticationController(ILogger<UserAuthenticationController> logger, IAuthService authService) : ControllerBase
{
  private readonly ILogger<UserAuthenticationController> _logger = logger;
  private readonly IAuthService _authService = authService;
  [HttpPost]
  public async Task<IActionResult> Index([FromBody] UserLogin model)
  {
    var user = await _authService.FetchUser(model);
    if (user == null)
    {
      _logger.LogError("Authentication failed.");
      return NotFound();
    }
    else
    {
      ClaimsPrincipal principal = _authService.BuildPrincipal(user);
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
      return Ok(user);
    }
  }
}