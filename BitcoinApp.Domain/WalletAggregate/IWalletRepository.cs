using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinApp.Domain.WalletAggregate
{
    public interface IWalletRepository
    {
        Task<Wallet> GetAsync(int id);

        Task UpdateAsync(Wallet wallet);

        Task<ICollection<Wallet>> GetAllAsync();
    }
}
