namespace KnowYourCustomerServiceForBank.Server.Models;

using System.ComponentModel.DataAnnotations;

public enum OnboardingStatusOptions
{
  NEW,
  IN_PROGRESS,
  COMPLETED
}

public enum UserRoleOptions
{
  CUSTOMER,
  ADMIN,
  KYC_OFFICER,
  COMPLIANCE_OFFICER
}
public class User
{
  [Key]
  public int UserId { get; set; }

  [Required]
  [StringLength(64)]
  [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
  public string FullName { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  public required string Email { get; set; }

  [Required]
  [StringLength(15)]
  [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits.")]
  public required string PhoneNumber { get; set; }

  [Required]
  [StringLength(64, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
  public string PasswordHash { get; set; } = string.Empty;

  public OnboardingStatusOptions OnboardingStatus { get; set; } = OnboardingStatusOptions.NEW;

  public UserRoleOptions UserRole { get; set; } = UserRoleOptions.CUSTOMER;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [Required]
  public DateTime DateOfBirth { get; set; }

  public bool IsActive { get; set; } = false;

}
