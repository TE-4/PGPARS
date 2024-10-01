
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

// Register the  repo with the IApplicantRepo interface
builder.Services.AddTransient<IApplicantRepository, ApplicantRepository>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// add transient here???

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

// add MapControllerRoute here???

app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard",
    defaults: new { Controller = "Admin", action = "Dashboard"});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapDefaultControllerRoute(); ???

app.MapRazorPages();

app.Run();
