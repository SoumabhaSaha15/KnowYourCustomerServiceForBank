using Microsoft.AspNetCore.Mvc;
using KnowYourCustomerServiceForBank.Server.Services;
using KnowYourCustomerServiceForBank.Server.Data;
namespace KnowYourCustomerServiceForBank.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController(ILogger<CustomerController> logger, ICustomerService customerService) : ControllerBase
{
  private readonly ICustomerService _customerService = customerService;
  private readonly ILogger<CustomerController> _logger = logger;
  // private readonly AppDbContext _context = context;
}
