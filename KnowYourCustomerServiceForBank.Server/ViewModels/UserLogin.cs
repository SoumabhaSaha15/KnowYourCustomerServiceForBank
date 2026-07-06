using System.ComponentModel.DataAnnotations;
namespace KnowYourCustomerServiceForBank.Server.ViewModels;

public class UserLogin
{
  [Required]
  [StringLength(254)]
  [EmailAddress]
  public required string Email { get; set; }

  [Required]
  [StringLength(256)]
  public required string Password { get; set; }
}