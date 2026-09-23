using KnowYourCustomerServiceForBank.Server.Data;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Validators;
using KnowYourCustomerServiceForBank.Server.Repositories;
namespace KnowYourCustomerServiceForBank.Server.Services;

public class CustomerService(AppDbContext context, ILogger<CustomerService> logger) : ICustomerService
{
  private readonly AppDbContext _context = context;
  private readonly ILogger<CustomerService> _logger = logger;

}