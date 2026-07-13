using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using KnowYourCustomerServiceForBank.Server.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace KnowYourCustomerServiceForBank.Server.Models;

public enum AccountTypeOptions
{
  MINOR,
  JOINT,
  SAVINGS,
  CURRENT,
  FIXED_DEPOSIT,
  RECURRING_DEPOSIT,
  NON_RESIDENT_EXTERNAL,
  NON_RESIDENT_ORDINARY,
}

public enum AccountStatusOptions
{
  PENDING_APPROVAL,
  ACTIVE,
  REJECTED,
  FROZEN,
  CLOSED
}
public class Account : ITrackable
{
  [Key]
  public int AccountId { get; set; }

  // [JsonConverter(typeof(JsonStringEnumConverter))]
  public AccountTypeOptions AccountType { get; set; } = AccountTypeOptions.SAVINGS;

  // [JsonConverter(typeof(JsonStringEnumConverter))]
  public AccountStatusOptions AccountStatus { get; set; } = AccountStatusOptions.PENDING_APPROVAL;

  public int UserId { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [ValidateNever]
  public User User { get; set; } = null!;
}