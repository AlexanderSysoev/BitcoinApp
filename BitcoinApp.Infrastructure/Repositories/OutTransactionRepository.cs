using System.Threading.Tasks;
using BitcoinApp.Domain.OutTransactionAggregate;

namespace BitcoinApp.Infrastructure.Repositories
{
    public class OutTransactionRepository : IOutTransactionRepository
    {
        private readonly BitcoinAppContext _bitcoinAppContext;

        public OutTransactionRepository(BitcoinAppContext bitcoinAppContext)
        {
            _bitcoinAppContext = bitcoinAppContext;
        }

        public async Task SaveAsync(OutTransaction outTransaction)
        {
            await _bitcoinAppContext.OutTransactions.AddAsync(outTransaction);
            _bitcoinAppContext.SaveChanges();
        }
    }
}
