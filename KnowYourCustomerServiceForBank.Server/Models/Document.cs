using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using KnowYourCustomerServiceForBank.Server.Interfaces;
namespace KnowYourCustomerServiceForBank.Server.Models;

public enum DocumentVerificationStatusOptions
{
  PENDING,
  VERIFIED,
  REJECTED
}

public enum DocumentTypeOptions
{
  AADHAR,
  PASSPORT,
  VOTER_ID,
  STUDENT_ID,
  DRIVER_LICENSE,
  BIRTH_CERTIFICATE,
  MARRIAGE_CERTIFICATE,
  PERMANENT_ACCOUNT_NUMBER,
}

public class Document : ITrackable
{
  [Key]
  public int DocumentId { get; set; }

  public int UserId { get; set; }

  [Required]
  public required DocumentTypeOptions DocumentType { get; set; }

  [Required]
  public required string FilePath { get; set; }

  public DocumentVerificationStatusOptions DocumentVerificationStatus { get; set; } = DocumentVerificationStatusOptions.PENDING;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [ValidateNever]
  public User User { get; set; } = null!;
}