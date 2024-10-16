using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Voting_App.Models;
using Voting_App.Services.ApplicationDataBase;
using Voting_App.Services.Authentication;
using Voting_App.Services.PasswordHash;
using Voting_App.Services.Voting;

namespace Voting_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "/Authentication/Login";
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<IAuthenticationService, CookieAuthenticationService>();
            builder.Services.AddTransient<IVoteService, VoteService>();
            builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

            var app = builder.Build();


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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

            using (var scope = app.Services.CreateScope())
            {
                InitDataBase(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
            }

            app.Run();
        }

        private static void InitDataBase(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();

            if (dbContext.Options.Count() == 0)
            {
                dbContext.Options.Add(new Option() { Name = "First", });
                dbContext.Options.Add(new Option() { Name = "Second", });
                dbContext.Options.Add(new Option() { Name = "Third", });

                dbContext.SaveChanges();
            }
        }
    }
}
