using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Data;
using Microsoft.AspNetCore.Identity;
namespace KnowYourCustomerServiceForBank.Server;

public static class SeedData
{
  public static async Task InitializeAsync(AppDbContext context)
  {
    if (context.Users.Any(u => u.Email == "admin@kycflow.com"))
      return;

    User adminUser = new()
    {
      FullName = "Admin",
      Email = "admin@kycflow.com",
      Password = "admin123", // In a real application, ensure to hash passwords securely
      UserRole = UserRoleOptions.ADMIN,
      OnboardingStatus = OnboardingStatusOptions.NOT_APPLICABLE,
      IsActive = true,
    };

    var hasher = new PasswordHasher<User>();
    adminUser.Password = hasher.HashPassword(adminUser, adminUser.Password);

    context.Users.Add(adminUser);
    await context.SaveChangesAsync();
  }
}

// Paste it in Program.cs
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//     _ = SeedData.InitializeAsync(context);
// }
