using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PGPARS.Services;
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
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();

// Register the FundingRepository with the interface; change me later :3
builder.Services.AddScoped<IFundingRepository, FundingRepository>();

// Register the ReviewRepository with the interface; change me later x)
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// Register the audit repository
builder.Services.AddScoped<IAuditRepository, AuditRepository>();

// Register custom services
builder.Services.AddTransient<CsvService>();
builder.Services.AddTransient<DbSeederService>();
builder.Services.AddTransient<ReviewAssignmentService>();
builder.Services.AddTransient<AuditLogService>();


// Add more services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().
AddRazorRuntimeCompilation();

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


// create a scope that uses services to create default users and user roles on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var DbSeeder = services.GetRequiredService<DbSeederService>();
    DbSeeder.SeedRolesAndUsers().Wait();


    // This will be changed to use the new REVIEW table instead of the previous ReviewAssignment table
    //var assignApplicants = services.GetRequiredService<ReviewAssignmentService>();
    //assignApplicants.AssignReviewers().Wait();

}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Logout}/{id?}");

//app.MapDefaultControllerRoute(); ???

app.MapRazorPages();

app.Run();
