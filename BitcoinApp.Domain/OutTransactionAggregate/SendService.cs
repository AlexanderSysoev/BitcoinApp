using System;
using System.Threading.Tasks;
using BitcoinApp.Domain.SeedWork;
using BitcoinApp.Domain.WalletAggregate;

namespace BitcoinApp.Domain.OutTransactionAggregate
{
    public class SendService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IOutTransactionRepository _outTransactionRepository;
        private readonly IBitcoinServiceFactory _bitcoinServiceFactory;

        public SendService(
            IWalletRepository walletRepository,
            IOutTransactionRepository outTransactionRepository,
            IBitcoinServiceFactory bitcoinServiceFactory)
        {
            _walletRepository = walletRepository;
            _outTransactionRepository = outTransactionRepository;
            _bitcoinServiceFactory = bitcoinServiceFactory;
        }

        public async Task<(bool, string)> SendBtcAsync(int fromWalletId, string toWalletAddress, decimal amount)
        {
            //TODO: get and update transaction
            var wallet = await _walletRepository.GetAsync(fromWalletId);
            if (wallet == null)
            {
                return (false, $"Wallet with Id={fromWalletId} not found");
            }

            if (!wallet.BalanceOut(amount))
            {
                return (false, $"Wallet with Id={fromWalletId} insufficient funds");
            }

            await _walletRepository.UpdateAsync(wallet);

            string txId;
            try
            {
                var bitcoinService = _bitcoinServiceFactory.CreateForWallet(wallet);

                txId = bitcoinService.SendToAddress(toWalletAddress, amount, string.Empty, string.Empty, false);
                if (string.IsNullOrEmpty(txId))
                {
                    throw new ApplicationException($"Failed to create out transaction from wallet with Id={wallet.Id}");
                }
            }
            catch (Exception ex)
            {
                wallet.BalanceIn(amount);
                await _walletRepository.UpdateAsync(wallet);
                return (false, ex.Message);
            }

            var outTransaction = new OutTransaction(txId, fromWalletId, toWalletAddress, amount);
            await _outTransactionRepository.SaveAsync(outTransaction);

            return (true, string.Empty);
        }
    }
}
