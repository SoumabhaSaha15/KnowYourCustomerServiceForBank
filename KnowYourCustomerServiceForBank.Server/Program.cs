using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using KnowYourCustomerServiceForBank.Server.Data;
namespace KnowYourCustomerServiceForBank.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "SessionCache";
            });

            // 2. Register Session State Services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);          // Session expiration length
                options.Cookie.HttpOnly = true;                          // Mitigates XSS security vulnerabilities
                options.Cookie.IsEssential = true;                       // Ensures cookie functions regardless of user consent
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Forces HTTPS delivery channels
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();



            var app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                    {
                        // Directs Swagger UI to use the native .NET 9 OpenAPI spec document
                        options.SwaggerEndpoint("/openapi/v1.json", "Banking Compliance API v1");
                    });
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
