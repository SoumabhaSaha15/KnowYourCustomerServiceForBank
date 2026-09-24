using System.Reflection; // REQUIRED for GetCustomAttribute
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using KnowYourCustomerServiceForBank.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Config;
using KnowYourCustomerServiceForBank.Server.Annotations;
namespace KnowYourCustomerServiceForBank.Server
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.Scan(
        (scan) =>
        {
          scan
            .FromAssemblies(typeof(Program).Assembly)
            .AddClasses(classes => classes.WithAttribute<ServiceLifetimeAttribute>())
            .AsImplementedInterfaces()
            .WithLifetime(type => type.GetCustomAttribute<ServiceLifetimeAttribute>()?.Lifetime ?? ServiceLifetime.Scoped);
        }
      );  // Adding scrutor reflection

      builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      builder.Services.AddDistributedSqlServerCache(
        (options) =>
        {
          options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
          options.SchemaName = "dbo";
          options.TableName = "SessionCache";
        }
      );  // Database session cache

      builder.Services.AddSession(
        (options) =>
        {
          options.IdleTimeout = TimeSpan.FromDays(1); // Session expiration length
          options.Cookie.HttpOnly = true; // Mitigates XSS security vulnerabilities
          options.Cookie.IsEssential = true; // Ensures cookie functions regardless of user consent
          options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Forces HTTPS delivery channels
        }
      );  // 2. Register Session State Services

      builder.Services
        .AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

      builder.Services.AddOpenApi();  // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

      builder.Services.AddSingleton<ITicketStore, DistributedCacheTicketStore>(); // Storing session in DB cache.

      builder.Services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(
          (options) =>
          {
            options.Events.OnRedirectToLogin = (context) =>
              {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
                return Task.CompletedTask;
              };
            options.Events.OnRedirectToAccessDenied = (context) =>
              {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
              };
            options.Cookie.HttpOnly = true; // Protect against XSS
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
            options.ExpireTimeSpan = TimeSpan.FromHours(8); // Set reasonable expiration
            options.SlidingExpiration = true;
          }
        );

      builder.Services
        .AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
        .Configure<ITicketStore>((options, store) => options.SessionStore = store);

      builder.Services
        .AddAuthorizationBuilder()
        .AddPolicy(
          "StaffOnly",
          (policy) =>
          {
            policy.RequireClaim(
              ClaimTypes.Role,
              UserRoleOptions.ADMIN.ToString(),
              UserRoleOptions.KYC_OFFICER.ToString(),
              UserRoleOptions.COMPLIANCE_OFFICER.ToString()
            );
          }
        );

      var app = builder.Build();

      app.UseDefaultFiles();
      app.MapStaticAssets();
      if (app.Environment.IsDevelopment())  // Configure the HTTP request pipeline.
      {
        app.MapOpenApi();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Banking Compliance API v1"));
      }
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseSession();
      app.UseAuthentication();
      app.UseAuthorization();
      app.MapControllers();
      app.MapFallbackToFile("/index.html");
      app.Run();
    }
  }
}
