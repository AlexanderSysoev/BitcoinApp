using System;
using System.Threading.Tasks;
using BitcoinApp.Domain.SeedWork;
using BitcoinApp.Domain.WalletAggregate;

namespace BitcoinApp.Domain.InTransactionAggregate
{
    /// <summary>
    /// Служба отслеживания новых транзакций на вход кошельков
    /// </summary>
    public class NewTrackService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IBitcoinServiceFactory _bitcoinServiceFactory;
        private readonly IInTransactionRepository _inTransactionRepository;

        public NewTrackService(
            IWalletRepository walletRepository,
            IBitcoinServiceFactory bitcoinServiceFactory,
            IInTransactionRepository inTransactionRepository)
        {
            _walletRepository = walletRepository;
            _bitcoinServiceFactory = bitcoinServiceFactory;
            _inTransactionRepository = inTransactionRepository;
        }

        public async Task TrackNewAsync()
        {
            try
            {
                var wallets = await _walletRepository.GetAllAsync();
                foreach (var wallet in wallets)
                {
                    var lastTransaction = await _inTransactionRepository.FindLastAsync(wallet.Id);

                    var bitcoinService = _bitcoinServiceFactory.CreateForWallet(wallet);

                    var listSinceBlockResponse = bitcoinService.ListSinceBlock(lastTransaction?.BlockHash);
                    foreach (var transaction in listSinceBlockResponse.Transactions)
                    {
                        var inTransaction = new InTransaction(transaction.TxId, wallet.Id, transaction.Amount, transaction.BlockHash);
                        inTransaction.UpdateConfirmations(transaction.Confirmations);
                        await _inTransactionRepository.SaveAsync(inTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log
            }
        }
    }
}
