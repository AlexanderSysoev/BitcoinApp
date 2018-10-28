using System.Collections.Generic;
using System.Threading.Tasks;
using BitcoinApp.Domain.WalletAggregate;
using Microsoft.EntityFrameworkCore;

namespace BitcoinApp.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly BitcoinAppContext _bitcoinAppContext;

        public WalletRepository(BitcoinAppContext bitcoinAppContext)
        {
            _bitcoinAppContext = bitcoinAppContext;
        }

        public async Task<Wallet> GetAsync(int id)
        {
            return await _bitcoinAppContext.Wallets.FindAsync(id);
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _bitcoinAppContext.Entry(wallet).State = EntityState.Modified;
            await _bitcoinAppContext.SaveChangesAsync();
        }

        public async Task<ICollection<Wallet>> GetAllAsync()
        {
            return await _bitcoinAppContext.Wallets.ToListAsync();
        }
    }
}
