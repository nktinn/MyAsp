using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class HstsUpdateService : BackgroundService
{
    private readonly IServiceProvider _services;

    public HstsUpdateService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Получаем IHttpContextAccessor из IServiceProvider
            using (var scope = _services.CreateScope())
            {
                var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;

                // Обновляем HSTS заголовок с новым временем действия (30 дней)
                httpContext.Response.Headers.Add("Strict-Transport-Security", "max-age=2592000");
            }

            // Ждем 30 дней перед следующим обновлением
            await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
        }
    }
}
