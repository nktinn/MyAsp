using MyAsp.Storage;

namespace MyAsp.Logic.HostedService
{
    public interface IHostedManager
    {
        public Task RemoveExpiredTempPasswords();
        public Task ParsePages();
        public Task DeleteExams();
        public Task DeleteExpiredGrants();
        public Task ClearNews();
    }
}
