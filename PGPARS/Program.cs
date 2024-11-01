
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PGPARS.Data;
using PGPARS.Models;

var builder = WebApplication.CreateBuilder(args);

// SQL DB 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Identity 
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register the ApplicantRepository with the interface; change me later :D
builder.Services.AddScoped<IApplicantRepository, FakeApplicantRepository>();

// Register the FundingRepository with the interface; change me later :3
builder.Services.AddScoped<IFundingRepository, FakeFundingRepository>();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Middleware pipeline setup
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// create a scope that uses services to admin user and user roles on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // call method from DbSeeder class to seed roles
    await DbSeeder.SeedAdminUser(userManager, roleManager);
}


// add MapControllerRoute here???

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard",
    defaults: new { Controller = "Admin", action = "Dashboard"});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

//app.MapDefaultControllerRoute(); ???

app.MapRazorPages();

app.Run();
