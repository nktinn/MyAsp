using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;

namespace MyAsp.HostedServices
{
    public class HangfireHostedService : IHostedService
    {
        private readonly IBackgroundJobClient _backgroundJobs;

        public HangfireHostedService(IBackgroundJobClient backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
        }
        public void ScheduleHangfireTasks()
        {
            RecurringJob.AddOrUpdate<BackgroundJob>("RemoveTempPasswords", x => x.ExecuteTaskRemoveTempPasswords(), "0 0 * * * *"); // Каждый час
            RecurringJob.AddOrUpdate<BackgroundJob>("ParsePages", x => x.ExecuteTaskParsePages(), "0 0 15 * * *"); // Каждый день в 15:00
            RecurringJob.AddOrUpdate<BackgroundJob>("DeleteExams", x => x.ExecuteTaskDeleteExams(), "0 30 1 1 9 *"); // 1 сентября
            RecurringJob.AddOrUpdate<BackgroundJob>("DeleteExams10", x => x.ExecuteTaskDeleteExams(), "0 30 1 10 2 *"); // 10 февраля
            RecurringJob.AddOrUpdate<BackgroundJob>("DeleteExpiredGrants", x => x.ExecuteTaskDeleteExpiredGrants(), "0 0 3 1 * *"); // Каждый месяц 1 числа
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ScheduleHangfireTasks();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
