using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using KnowYourCustomerServiceForBank.Server.Interfaces;
namespace KnowYourCustomerServiceForBank.Server.Models;

public enum OnboardingStatusOptions
{
  NEW,
  IN_PROGRESS,
  COMPLETED,
  NOT_APPLICABLE
}

public enum UserRoleOptions
{
  CUSTOMER,
  ADMIN,
  KYC_OFFICER,
  COMPLIANCE_OFFICER
}
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class User : ITrackable
{
  [Key]
  public int UserId { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 2)]
  [RegularExpression(@"^[a-zA-Z\s'.\-]+$", ErrorMessage = "Name contains invalid characters.")]
  public required string FullName { get; set; }

  [Required]
  [StringLength(254)]
  [EmailAddress]
  public required string Email { get; set; }

  [StringLength(20)]
  [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be valid.")]
  public string? PhoneNumber { get; set; }

  [StringLength(255)]
  [RegularExpression(@"^[a-zA-Z\s'.\-]+$", ErrorMessage = "Address contains invalid characters.")]
  public string? Address { get; set; }

  [Required]
  [StringLength(256)]
  [JsonIgnore]
  public string Password { get; set; } = null!;

  [Required]
  // [JsonConverter(typeof(JsonStringEnumConverter))]
  public required OnboardingStatusOptions OnboardingStatus { get; set; }

  public DateOnly? DateOfBirth { get; set; }

  // [JsonConverter(typeof(JsonStringEnumConverter))]
  public UserRoleOptions UserRole { get; set; } = UserRoleOptions.CUSTOMER;

  public bool IsActive { get; set; } = false; // Default to false so seeded officers cannot log in until activated

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [ValidateNever]
  [JsonIgnore]
  public IEnumerable<Document> Documents { get; set; } = [];

  [ValidateNever]
  [JsonIgnore]
  public IEnumerable<Account> Accounts { get; set; } = [];

  [ValidateNever]
  [JsonIgnore]
  public IEnumerable<AuditLog> AuditLogs { get; set; } = [];

  [ValidateNever]
  [JsonIgnore]
  public RiskProfile? RiskProfile { get; set; } = null;

}