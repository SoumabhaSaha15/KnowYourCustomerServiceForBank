using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Services;
using KnowYourCustomerServiceForBank.Server.ViewModels;

namespace KnowYourCustomerServiceForBank.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserLoginController(ILogger<UserLoginController> logger, ILoginService loginService) : ControllerBase
{
  private readonly ILogger<UserLoginController> _logger = logger;
  private readonly ILoginService _loginService = loginService;
  [HttpPost]
  public async Task<IActionResult> Index([FromBody] UserLogin model)
  {
    var user = await _loginService.FetchUser(model);
    if (user == null)
    {
      _logger.LogError("Authentication failed.");
      return NotFound();
    }
    else
    {
      ClaimsPrincipal principal = _loginService.BuildPrincipal(user);
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
      return Ok(user);
    }
  }
}