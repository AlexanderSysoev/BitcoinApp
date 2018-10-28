using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinApp.Domain.InTransactionAggregate
{
    public interface IInTransactionRepository
    {
        Task<ICollection<InTransaction>> FindAsync(int maxConfirmationsCount);

        Task<ICollection<InTransaction>> FindFromAsync(int lastId, int maxConfirmationsCount);

        Task<InTransaction> FindLastAsync(int walletId);

        Task UpdateAsync(InTransaction inTransaction);

        Task SaveAsync(InTransaction inTransaction);
    }
}
