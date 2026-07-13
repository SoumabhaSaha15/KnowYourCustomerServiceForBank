using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KnowYourCustomerServiceForBank.Server.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace KnowYourCustomerServiceForBank.Server.Models;

public enum AuditActionOptions
{
  USER_CREATED,
  USER_UPDATED,
  USER_DELETED,
  DOCUMENT_UPLOADED,
  DOCUMENT_VERIFIED,
  DOCUMENT_REJECTED,
  ACCOUNT_CREATED,
  ACCOUNT_APPROVED,
  ACCOUNT_REJECTED,
  ACCOUNT_FROZEN,
  ACCOUNT_CLOSED,
  RISK_PROFILE_UPDATED
}

public class AuditLog : ITrackable
{
  [Key]
  public int AuditLogId { get; set; }

  public int UserId { get; set; }

  [Required]
  // [JsonConverter(typeof(JsonStringEnumConverter))]
  public required AuditActionOptions Action { get; set; }

  [Required]
  [StringLength(256)]
  public required string Remarks { get; set; } // Optional field for additional information about the action

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [ValidateNever]
  public User User { get; set; } = null!;
}