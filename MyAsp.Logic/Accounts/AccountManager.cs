using MyAsp.Storage;

namespace MyAsp.Logic.Accounts
{
    public class AccountManager: IAccountManager
    {

        private readonly Context _context;
        public AccountManager(Context context)
        {
            _context = context;
        }

        public async Task UpdateContext()
        {
            await _context.SaveChangesAsync();
        }
    }
}
