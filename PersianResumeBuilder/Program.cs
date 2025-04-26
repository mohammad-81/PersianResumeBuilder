using Microsoft.AspNetCore.Authentication.Cookies;
using PersianResumeBuilder.DataBase;

namespace PersianResumeBuilder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<Sample_DbContext>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";  // مسیر صفحه ورود
                options.LogoutPath = "/Account/Logout"; // مسیر خروج
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // زمان اعتبار کوکی (۷ روز)
                options.SlidingExpiration = true; // تمدید اعتبار در صورت فعالیت کاربر
            });
            builder.Services.AddAuthorization(); // فعال‌سازی سیستم مجوزدهی


            var app = builder.Build();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
