using System.Reflection; // REQUIRED for GetCustomAttribute
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using KnowYourCustomerServiceForBank.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Annotations;
namespace KnowYourCustomerServiceForBank.Server
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      // Add services to the container.
      // using KnowYourCustomerServiceForBank.Server.Annotations;

      builder.Services.Scan(scan => scan
          .FromAssemblies(typeof(Program).Assembly)
          .AddClasses(classes => classes.WithAttribute<ServiceLifetimeAttribute>())
              .AsImplementedInterfaces()
              .WithLifetime(
                (type) =>
                  {
                    return type.GetCustomAttribute<ServiceLifetimeAttribute>()?.Lifetime ?? ServiceLifetime.Scoped;
                  }
              )
      );

      builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
      );
      builder.Services.AddDistributedSqlServerCache(options =>
      {
        options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.SchemaName = "dbo";
        options.TableName = "SessionCache";
      });

      // 2. Register Session State Services
      builder.Services.AddSession(
        (options) =>
        {
          options.IdleTimeout = TimeSpan.FromDays(1); // Session expiration length
          options.Cookie.HttpOnly = true; // Mitigates XSS security vulnerabilities
          options.Cookie.IsEssential = true; // Ensures cookie functions regardless of user consent
          options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Forces HTTPS delivery channels
        }
      );


      builder.Services
      .AddControllers()
      .AddJsonOptions(
        (options) => options.JsonSerializerOptions.Converters.Add(
          new System.Text.Json.Serialization.JsonStringEnumConverter()
        )
      );

      // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      builder.Services.AddOpenApi();
      builder.Services
          .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie(options =>
          {
            options.Events.OnRedirectToLogin = (context) =>
                  {
                    context.Response.StatusCode = 401; // Unauthorized
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
          });
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
