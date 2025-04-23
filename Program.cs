using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DnDWebpage.Data;
using DnDWebpage.Models;

namespace DnDWebpage
{
    public class Program
    {
        public static async Task Main(string[] args)

        {
            var builder = WebApplication.CreateBuilder(args);

            // ?? Register Razor Pages for Identity UI and Account Management
            builder.Services.AddRazorPages();

            // ?? Connect to Azure-hosted SQL Database using EF Core
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                 sqlOptions => sqlOptions.EnableRetryOnFailure()
     ));


            // ??? Configure Identity with your custom ApplicationUser
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Optional: Relax password strength here if desired
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;

                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // ?? Add support for Controllers + Views
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ?? Enable authentication spells and protective authorization wards
            app.UseAuthentication();
            app.UseAuthorization();

            // ?? Define the main route for MVC controller navigation
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ?? Enable Razor Pages (required for Identity UI like Login, Register, etc.)
            app.MapRazorPages();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await RoleInitializer.SeedRolesAndAdmin(services);
            }

            app.Run();
        }
    }
}
