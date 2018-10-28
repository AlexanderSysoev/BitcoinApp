using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoinApp.Domain.InTransactionAggregate;
using Microsoft.EntityFrameworkCore;

namespace BitcoinApp.Infrastructure.Repositories
{
    public class InTransactionRepository : IInTransactionRepository
    {
        private readonly BitcoinAppContext _bitcoinAppContext;

        public InTransactionRepository(BitcoinAppContext bitcoinAppContext)
        {
            _bitcoinAppContext = bitcoinAppContext;
        }

        public async Task<ICollection<InTransaction>> FindAsync(int maxConfirmationsCount)
        {
            return await _bitcoinAppContext.InTransactions
                .Where(t => t.ConfirmationsCount < maxConfirmationsCount)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<ICollection<InTransaction>> FindFromAsync(int lastId, int maxConfirmationsCount)
        {
            return await _bitcoinAppContext.InTransactions
                .Where(t => t.ConfirmationsCount < maxConfirmationsCount || t.Id > lastId)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<InTransaction> FindLastAsync(int walletId)
        {
            return await _bitcoinAppContext.InTransactions
                .Where(t => t.ToWalletId == walletId)
                .OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(InTransaction inTransaction)
        {
            _bitcoinAppContext.Entry(inTransaction).State = EntityState.Modified;
            await _bitcoinAppContext.SaveChangesAsync();
        }

        public async Task SaveAsync(InTransaction inTransaction)
        {
            await _bitcoinAppContext.InTransactions.AddAsync(inTransaction);
            _bitcoinAppContext.SaveChanges();
        }
    }
}
