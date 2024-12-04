using DashBoard_MotoManager.Datas;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using DashBoard_MotoManager.Configurations;

namespace DashBoard_MotoManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MotoWebsiteContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DashboardWeb"));
            });
            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                option =>
                {
                    option.LoginPath = "/User/SignIn";
                    option.AccessDeniedPath = "/AccessDenied";
                });
            builder.Services.AddControllers().
                AddNewtonsoftJson(options => 
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();
            app.Use(async (context, next) =>
            {
                try
                {
                    // Proceed to the next middleware
                    await next();
                }
                catch (BadHttpRequestException ex)
                {
                    // Handle 400 Bad Request
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Bad request error.");

                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("Bad Request: The request is invalid.");
                }
                catch (UnauthorizedAccessException ex)
                {
                    // Handle 401 Unauthorized
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Unauthorized access error.");

                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Unauthorized: Access is denied due to invalid credentials.");
                }
                catch (KeyNotFoundException ex)
                {
                    // Handle 404 Not Found
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Resource not found.");

                    context.Response.StatusCode = 404; // Not Found
                    await context.Response.WriteAsync("Not Found: The requested resource could not be found.");
                }
                catch (TimeoutException ex)
                {
                    // Handle 504 Gateway Timeout
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Request timeout error.");

                    context.Response.StatusCode = 504; // Gateway Timeout
                    await context.Response.WriteAsync("Gateway Timeout: The server took too long to respond.");
                }
                catch (Exception ex)
                {
                    // Handle generic 500 Internal Server Error
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An unhandled exception occurred.");

                    context.Response.StatusCode = 500; // Internal Server Error
                    await context.Response.WriteAsync("Internal Server Error: An unexpected error occurred.");
                }
            });



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
