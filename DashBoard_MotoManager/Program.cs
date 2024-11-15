using DashBoard_MotoManager.Datas;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                option =>
                {
                    option.LoginPath = "/User/SignIn";
                    option.AccessDeniedPath = "/AccessDenied";
                });
            var app = builder.Build();
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Lỗi tổng quát xảy ra trong middleware.");

                    // Trả về mã lỗi và thông báo cho client nếu cần
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Đã xảy ra lỗi không mong muốn.");
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
