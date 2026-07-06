using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using KnowYourCustomerServiceForBank.Server.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace KnowYourCustomerServiceForBank.Server.Models;

public enum RiskLevelOptions
{
  LOW,
  MEDIUM,
  HIGH
}
public class RiskProfile : ITrackable
{
  [Key]
  public int RiskProfileId { get; set; }

  public int UserId { get; set; }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public RiskLevelOptions RiskLevel { get; set; } = RiskLevelOptions.LOW; // e.g., Low, Medium, High

  public double? Score { get; set; } // e.g., List of risk factors identified

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [ValidateNever]
  public User User { get; set; } = null!;
}