namespace MyAsp.Logic.Accounts
{
    public interface IAccountManager
    {
        public Task UpdateContext();
        public Task ClearNews();
        public Task ParseStankinRU();
    }
}
