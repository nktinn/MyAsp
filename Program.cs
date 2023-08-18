using Microsoft.EntityFrameworkCore;
using MyAsp.Logic.Accounts;
using MyAsp.Logic.Files;
using MyAsp.Storage;
using MyAsp.Storage.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();
services.AddScoped<IAccountManager, AccountManager>();
services.AddScoped<IAspAccountManager, AspAccountManager>();
services.AddScoped<IAdmAccountManager, AdmAccountManager>();
services.AddScoped<IFileManager, FileManager>();

// Add DataBase context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));


/*
// Добавляем IHttpContextAccessor
services.AddHttpContextAccessor();

// Добавляем HstsUpdateService как периодическую задачу
services.AddHostedService<HstsUpdateService>();
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Asp}/{action=Login}");
app.Run();
