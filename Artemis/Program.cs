/*
* Setup Application Middleware
*/

using Artemis.Data;
using Artemis.Infrastructure.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string identityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection")
    ?? throw new InvalidOperationException("Connection string 'IdentityConnection' not found!");

string artemisConnectionString = builder.Configuration.GetConnectionString("ArtemisConnection")
    ?? throw new InvalidOperationException("Connection string 'ArtemisConnection' not found!");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

//builder.Services.AddDbContext<ArtemisDbContext>(otions => 
//    otions.UseSqlServer(artemisConnectionString));

builder.Services.AddArtemisDbContext(artemisConnectionString);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    // Password settings.
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+!";
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders()
.AddRoles<ApplicationRole>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Below are the default values (except ExpireTimeSpan=5) and can be omitted.
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddKendo();

// Enable Google-Account login
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientId")!;
    googleOptions.ClientSecret = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientSecret")!;
});

builder.Services.AddAuthentication().AddMicrosoftAccount(micorosoftOptions =>
{
    micorosoftOptions.ClientId = builder.Configuration.GetSection("MicrosoftAccountSettings").GetValue<string>("AppId")!;
    micorosoftOptions.ClientSecret = builder.Configuration.GetSection("MicrosoftAccountSettings").GetValue<string>("AppSecret")!;
});

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Pages/Partials/{0}" + RazorViewEngine.ViewExtension);
});

builder.Services.AddArtemisRepositories();

var app = builder.Build();

app.UseConfigurationCulture();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
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

app.MapRazorPages();

app.MapDefaultControllerRoute();

// Seed the Identity database with initial values for roles and administrator
IdentitySeed.CreateRoles(app.Services);
IdentitySeed.CreateAdminAccount(app.Services, app.Configuration);

app.Run();
