using Microsoft.EntityFrameworkCore;
using MyAsp.Logic.Accounts;
using MyAsp.Logic.Files;
using MyAsp.Logic.HostedService;
using MyAsp.Storage;
using MyAsp.HostedServices;
using MyAsp.Storage.Entities;

using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllersWithViews();
services.AddScoped<IAccountManager, AccountManager>();
services.AddScoped<IAspAccountManager, AspAccountManager>();
services.AddScoped<IAdmAccountManager, AdmAccountManager>();
services.AddScoped<IFileManager, FileManager>();
services.AddScoped<IHostedManager, HostedManager>();



// Add DataBase context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));


services.AddHostedService<HangfireHostedService>();
services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseMemoryStorage()
        .UseRecommendedSerializerSettings());

services.AddHangfireServer();

services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(730); // 2 года
    options.IncludeSubDomains = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Asp}/{action=Login}");

app.Run();
