using FluentValidation;
using KnowYourCustomerServiceForBank.Server.Models;
namespace KnowYourCustomerServiceForBank.Server.Validators;

public class CreateCustomer : AbstractValidator<User>
{
  public CreateCustomer()
  {
    When(x => x.UserRole == UserRoleOptions.CUSTOMER, () =>
    {
      RuleFor(x => x.PhoneNumber)
      .NotEmpty()
      .WithMessage("Phone number is required for customers.");

      RuleFor(x => x.OnboardingStatus)
      .NotEqual(OnboardingStatusOptions.NOT_APPLICABLE)
      .WithMessage("Onboarding status can't be 'NOT_APPLICABLE'");

      RuleFor(x => x.DateOfBirth)
        .Cascade(CascadeMode.Stop)
        .NotEmpty()
        .WithMessage("Date of birth is required.")
        .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
        .WithMessage("Date of birth must be in the past.");

      RuleFor(x => x.Address)
        .NotEmpty()
        .WithMessage("Address is required for customers.");
    });
  }
}

