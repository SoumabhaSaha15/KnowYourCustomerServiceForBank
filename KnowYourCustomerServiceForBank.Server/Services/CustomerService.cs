using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Validators;
using KnowYourCustomerServiceForBank.Server.Repositories;
namespace KnowYourCustomerServiceForBank.Server.Services;

public class CustomerService(IRepository<User> repository, ILogger<CustomerService> logger) : ICustomerService
{
  private readonly IRepository<User> _repository = repository;
  private readonly ILogger<CustomerService> _logger = logger;

}