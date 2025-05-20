using DataAccess.Data;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Services.Services;
using Services.Services.Interfaces;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddDistributedMemoryCache(); 
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });


        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<DbHelper>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IFoodItemService, FoodItemService>();
        builder.Services.AddScoped<IGoalService, GoalService>();
        builder.Services.AddScoped<IActivityLevelService, ActivityLevelService>();
        builder.Services.AddScoped<IMetricService, MetricService>();
        builder.Services.AddScoped<IGenderService, GenderService>();
        builder.Services.AddScoped<AiService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<OpenFoodFactsService>();




        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped<IGoalRepository, GoalRepository>();
        builder.Services.AddScoped<IActivityLevelRepository, ActivityLevelRepository>();
        builder.Services.AddScoped<IMetricRepository, MetricRepository>();
        builder.Services.AddScoped<IGenderRepository, GenderRepository>();
        builder.Services.AddScoped<IFoodDiaryRepository, FoodDiaryRepository>();
        builder.Services.AddScoped<IFoodDiaryService, FoodDiaryService>();
        builder.Services.AddScoped<IMealTypeRepository, MealTypeRepository>();
        builder.Services.AddScoped<IMealTypeService, MealTypeService>();
        builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
        builder.Services.AddScoped<IFoodItemService, FoodItemService>();


        var app = builder.Build();


        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseSession();


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


        app.Run();
    }
}
