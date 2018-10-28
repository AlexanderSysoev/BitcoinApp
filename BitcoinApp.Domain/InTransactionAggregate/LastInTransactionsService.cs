using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace BitcoinApp.Domain.InTransactionAggregate
{
    public class LastInTransactionsService
    {
        private const int MaxConfirmationsCount = 3;
        private readonly IInTransactionRepository _inTransactionRepository;
        private readonly IMemoryCache _memoryCache;

        public LastInTransactionsService(
            IInTransactionRepository inTransactionRepository,
            IMemoryCache memoryCache)
        {
            _inTransactionRepository = inTransactionRepository;
            _memoryCache = memoryCache;
        }

        public async Task<ICollection<InTransaction>> GetLastAsync()
        {
            if (!_memoryCache.TryGetValue("lastId", out int lastId))
            {
                lastId = 0;
            }

            var lastTransactions = await _inTransactionRepository.FindFromAsync(lastId, MaxConfirmationsCount);

            if (lastTransactions != null && lastTransactions.Any())
            {
                _memoryCache.Set("lastId", lastTransactions.Last().Id);
            }

            return lastTransactions ?? new Collection<InTransaction>();
        }
    }
}
