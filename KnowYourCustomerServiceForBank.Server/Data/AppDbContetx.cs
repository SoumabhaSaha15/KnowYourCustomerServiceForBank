using Microsoft.EntityFrameworkCore;
using KnowYourCustomerServiceForBank.Server.Models;
namespace KnowYourCustomerServiceForBank.Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }
}
