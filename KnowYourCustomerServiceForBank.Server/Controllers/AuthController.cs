using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using KnowYourCustomerServiceForBank.Server.Services;
using KnowYourCustomerServiceForBank.Server.ViewModels;
namespace KnowYourCustomerServiceForBank.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(ILogger<AuthController> logger, IAuthService authService) : ControllerBase
{
  private readonly ILogger<AuthController> _logger = logger;
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
      string token = _authService.GenerateAccessToken(user);
      HttpContext.Response.Headers["x-access-token"] = token;
      await HttpContext.SignInAsync(principal);
      return Ok(user);
    }
  }
}