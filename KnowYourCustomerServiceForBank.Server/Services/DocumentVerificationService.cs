using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Repositories;

namespace KnowYourCustomerServiceForBank.Server.Services;

public class DocumentVerificationService(ILogger<DocumentVerificationService> logger) : IDocumentVerificationService
{
  private readonly ILogger<DocumentVerificationService> _logger = logger;
}
