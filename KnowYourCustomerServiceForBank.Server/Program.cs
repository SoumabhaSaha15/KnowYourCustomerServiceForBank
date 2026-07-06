using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using KnowYourCustomerServiceForBank.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Services;
using KnowYourCustomerServiceForBank.Server.Repositories;

namespace KnowYourCustomerServiceForBank.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = builder.Configuration.GetConnectionString(
                    "DefaultConnection"
                );
                options.SchemaName = "dbo";
                options.TableName = "SessionCache";
            });

            // 2. Register Session State Services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1); // Session expiration length
                options.Cookie.HttpOnly = true; // Mitigates XSS security vulnerabilities
                options.Cookie.IsEssential = true; // Ensures cookie functions regardless of user consent
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Forces HTTPS delivery channels
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = "/user-login"; });
            builder.Services
                .AddAuthorizationBuilder()
                .AddPolicy("StaffOnly", policy =>
                    policy.RequireClaim(
                        ClaimTypes.Role,
                        UserRoleOptions.ADMIN.ToString(),
                        UserRoleOptions.KYC_OFFICER.ToString(),
                        UserRoleOptions.COMPLIANCE_OFFICER.ToString()
                    )
                );
            var app = builder.Build();
            // Chain the next policy cleanly right underneath it

            app.UseDefaultFiles();
            app.MapStaticAssets();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Banking Compliance API v1"));
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
