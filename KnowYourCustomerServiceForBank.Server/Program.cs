using System.Reflection; // REQUIRED for GetCustomAttribute
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;
using KnowYourCustomerServiceForBank.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using KnowYourCustomerServiceForBank.Server.Models;
using KnowYourCustomerServiceForBank.Server.Filters;
// using KnowYourCustomerServiceForBank.Server.Services;
using KnowYourCustomerServiceForBank.Server.Annotations;
using KnowYourCustomerServiceForBank.Server.Repositories;

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
                    .WithLifetime(type =>
                        type.GetCustomAttribute<ServiceLifetimeAttribute>()?.Lifetime ?? ServiceLifetime.Scoped)
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
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1); // Session expiration length
                options.Cookie.HttpOnly = true; // Mitigates XSS security vulnerabilities
                options.Cookie.IsEssential = true; // Ensures cookie functions regardless of user consent
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Forces HTTPS delivery channels
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<AntiforgeryValidationFilter>();

            builder.Services.AddControllers(options =>
            {
                // Resolves AntiforgeryValidationFilter from DI for every request
                options.Filters.Add<AntiforgeryValidationFilter>();
            })
            .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()); });
            // builder.Services.AddScoped<ILoginService, LoginService>();
            // builder.Services.AddControllers()

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
                    options.Cookie.SameSite = SameSiteMode.Lax; // Protect against CSRF
                    options.ExpireTimeSpan = TimeSpan.FromHours(8); // Set reasonable expiration
                    options.SlidingExpiration = true;
                });
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
            builder.Services.AddAntiforgery(options =>
                {
                    options.HeaderName = "X-XSRF-TOKEN"; // Common header name for SPAs / Axios
                });
            var app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Banking Compliance API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // app.Use(async (context, next) =>
            //     {
            //         var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
            //         var tokens = antiforgery.GetAndStoreTokens(context);

            //         // HttpOnly MUST be false so React/Axios/Postman can read it
            //         context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
            //         {
            //             HttpOnly = false,
            //             Secure = true,
            //             SameSite = SameSiteMode.Lax
            //         });

            //         await next(context);
            //     });
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
