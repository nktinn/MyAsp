using System;
using System.Threading.Tasks;

using MyAsp.Storage;
using MyAsp.Logic.HostedService;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.States;

namespace MyAsp.HostedServices
{
    public class BackgroundJob
    {
        private readonly IHostedManager _hostmngr;

        public BackgroundJob(IHostedManager hostmngr)
        {
            _hostmngr = hostmngr;
        }

        public async Task ExecuteTaskRemoveTempPasswords()
        {
            await _hostmngr.RemoveExpiredTempPasswords();
        }
        public async Task ExecuteTaskParsePages()
        {
            await _hostmngr.ParsePages();
        }
        public async Task ExecuteTaskDeleteExams()
        {
            await _hostmngr.DeleteExams();
        }
        public async Task ExecuteTaskDeleteExpiredGrants()
        {
            await _hostmngr.DeleteExpiredGrants();
        }
    }
}
