using BitcoinApp.Domain.WalletAggregate;
using BitcoinLib.Services.Coins.Bitcoin;
using Microsoft.Extensions.Configuration;

namespace BitcoinApp.Domain.SeedWork
{
    public class BitcoinServiceFactory : IBitcoinServiceFactory
    {
        private readonly IConfiguration _configuration;

        public BitcoinServiceFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IBitcoinService CreateForWallet(Wallet wallet)
        {
            return new BitcoinService(
                daemonUrl: wallet.Address,
                rpcUsername: _configuration["RpcSettings:Username"],
                rpcPassword: _configuration["RpcSettings:Password"],
                walletPassword: wallet.Password,
                rpcRequestTimeoutInSeconds: 2);
        }
    }
}
