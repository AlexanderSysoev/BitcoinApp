using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinApp.Domain.SeedWork;
using BitcoinApp.Domain.WalletAggregate;
using BitcoinLib.Services.Coins.Bitcoin;
using Microsoft.Extensions.Caching.Memory;

namespace BitcoinApp.Domain.InTransactionAggregate
{
    /// <summary>
    /// Служба отслеживания подтверждений транзакций на вход кошельков
    /// </summary>
    public class ConfirmationTrackService
    {
        private const int MaxDegreeOfParallelism = 100;
        private const int MaxConfirmationsCount = 6;

        private readonly IBitcoinServiceFactory _bitcoinServiceFactory;
        private readonly IWalletRepository _walletRepository;
        private readonly IInTransactionRepository _inTransactionRepository;
        private readonly IMemoryCache _memoryCache;

        public ConfirmationTrackService(
            IBitcoinServiceFactory bitcoinServiceFactory,
            IWalletRepository walletRepository,
            IInTransactionRepository inTransactionRepository,
            IMemoryCache memoryCache)
        {
            _bitcoinServiceFactory = bitcoinServiceFactory;
            _walletRepository = walletRepository;
            _inTransactionRepository = inTransactionRepository;
            _memoryCache = memoryCache;
        }

        public async Task TrackConfirmationsAsync()
        {
            var inTransactions = await _inTransactionRepository.FindAsync(MaxConfirmationsCount);

            var semaphore = new SemaphoreSlim(MaxDegreeOfParallelism);

            var tasks = new List<Task>();

            foreach (var inTransaction in inTransactions)
            {
                var task = Task.Run(async () =>
                {
                    try
                    {
                        await semaphore.WaitAsync();
                        await ProcessInTransactionAsync(inTransaction);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private async Task ProcessInTransactionAsync(InTransaction inTransaction)
        {
            try
            {
                IBitcoinService bitcoinService;
                if (_memoryCache.TryGetValue($"BitcoinService_{inTransaction.ToWalletId}", out var service))
                {
                    bitcoinService = service as IBitcoinService;
                }
                else
                {
                    var wallet = await _walletRepository.GetAsync(inTransaction.ToWalletId);
                    bitcoinService = _bitcoinServiceFactory.CreateForWallet(wallet);
                    _memoryCache.Set($"BitcoinService_{inTransaction.ToWalletId}", bitcoinService);
                }

                var getTransactionResponse = bitcoinService.GetTransaction(inTransaction.TxId);
                inTransaction.UpdateConfirmations(getTransactionResponse.Confirmations);
                await _inTransactionRepository.UpdateAsync(inTransaction);
            }
            catch (Exception ex)
            {
                //Log
            }
        }
    }
}
