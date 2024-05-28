using FarmersApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Access configurationvar
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
// Added after the identity to recognise the pages as razor pages
builder.Services.AddRazorPages();


// For connection string
// Retrieve the connection string
var connectionString = configuration.GetConnectionString("farmers"); // Register your DbContext with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Making sure that both roles are added to the database.
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//For the razor pages same as the one on top
app.MapRazorPages(); //for Identity
app.Run();
