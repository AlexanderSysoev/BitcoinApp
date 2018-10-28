using BitcoinApp.Domain.WalletAggregate;
using BitcoinLib.Services.Coins.Bitcoin;

namespace BitcoinApp.Domain.SeedWork
{
    public interface IBitcoinServiceFactory
    {
        IBitcoinService CreateForWallet(Wallet wallet);
    }
}
