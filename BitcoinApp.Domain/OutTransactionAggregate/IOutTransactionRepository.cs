using System.Threading.Tasks;

namespace BitcoinApp.Domain.OutTransactionAggregate
{
    public interface IOutTransactionRepository
    {
        Task SaveAsync(OutTransaction outTransaction);
    }
}
