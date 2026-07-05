using Microsoft.EntityFrameworkCore;
using KnowYourCustomerServiceForBank.Server.Models;
namespace KnowYourCustomerServiceForBank.Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    var userEntity = modelBuilder.Entity<User>();
    #region UserTableEnumToStringConversion
    userEntity
      .Property(user => user.UserRole)
      .HasConversion(
        v => v.ToString(),
        v => Enum.Parse<UserRoleOptions>(v)
      );

    userEntity
      .Property(user => user.OnboardingStatus)
      .HasConversion(
        v => v.ToString(),
        v => Enum.Parse<OnboardingStatusOptions>(v)
      );
    #endregion

    #region AccountTableEnumToStringConversion
    modelBuilder.Entity<Account>()
      .Property(account => account.AccountType)
      .HasConversion(
        v => v.ToString(),
        v => Enum.Parse<AccountTypeOptions>(v)
      );

    modelBuilder.Entity<Account>()
      .Property(account => account.AccountStatus)
      .HasConversion(
        v => v.ToString(),
        v => Enum.Parse<AccountStatusOptions>(v)
      );
    #endregion

    #region DocumentTableEnumToStringConversion
    modelBuilder.Entity<Document>()
    .Property(doc => doc.DocumentType)
    .HasConversion(
      v => v.ToString(),
      v => Enum.Parse<DocumentTypeOptions>(v)
    );

    modelBuilder.Entity<Document>()
    .Property(doc => doc.DocumentVerificationStatus)
    .HasConversion(
      v => v.ToString(),
      v => Enum.Parse<DocumentVerificationStatusOptions>(v)
    );
    #endregion

    #region AuditLogTableEnumToStringConversion
    modelBuilder.Entity<AuditLog>()
    .Property(al => al.Action)
    .HasConversion(
      v => v.ToString(),
      v => Enum.Parse<AuditActionOptions>(v)
    );
    #endregion

    #region RiskProfileTableEnumToStringConversion
    modelBuilder.Entity<RiskProfile>()
    .Property(rp => rp.RiskLevel)
    .HasConversion(
      v => v.ToString(),
      v => Enum.Parse<RiskLevelOptions>(v)
    );
    #endregion

    #region ChangingDeleteBehaviour
    userEntity
      .HasOne(u => u.RiskProfile)
      .WithOne(rp => rp.User)
      .HasForeignKey<RiskProfile>(rp => rp.UserId)
      .OnDelete(DeleteBehavior.Restrict);

    userEntity
      .HasMany(u => u.Accounts)
      .WithOne(ac => ac.User)
      .HasForeignKey(ac => ac.UserId)
      .OnDelete(DeleteBehavior.Restrict);

    userEntity
      .HasMany(u => u.Documents)
      .WithOne(ac => ac.User)
      .HasForeignKey(ac => ac.UserId)
      .OnDelete(DeleteBehavior.Restrict);

    userEntity
      .HasMany(u => u.AuditLogs)
      .WithOne(ac => ac.User)
      .HasForeignKey(ac => ac.UserId)
      .OnDelete(DeleteBehavior.Restrict);
    #endregion

  }
  public DbSet<User> Users => Set<User>();
  public DbSet<Document> Documents => Set<Document>();
  public DbSet<Account> Accounts => Set<Account>();
  public DbSet<RiskProfile> RiskProfiles => Set<RiskProfile>();
  public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
}
