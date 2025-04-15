using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories;
using MealPulse.Data.Interfaces;
using MealPulse.Data.Repositories;
using MealPulse.Data;
using MealPulse.Services.Interfaces;
using MealPulse.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Services.Services.Interfaces;
using Services.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Add authentication services
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });

        // 🔧 Dependency Injection Setup
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<DbHelper>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>(); // Ensure it's here only once
        builder.Services.AddScoped<IFoodItemService, FoodItemService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IGoalService, GoalService>();
        builder.Services.AddScoped<IGoalRepository, GoalRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
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

        app.Run();
    }
}
