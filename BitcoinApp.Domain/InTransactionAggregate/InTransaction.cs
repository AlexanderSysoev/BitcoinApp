namespace BitcoinApp.Domain.InTransactionAggregate
{
    public class InTransaction
    {
        public InTransaction(string txId, int toWalletId, decimal amount, string blockHash)
        {
            TxId = txId;
            ToWalletId = toWalletId;
            Amount = amount;
            BlockHash = blockHash;
        }

        public int Id { get; set; }

        public string TxId { get; }

        public int ToWalletId { get; }

        public decimal Amount { get; }

        public string BlockHash { get; }

        public int ConfirmationsCount { get; private set; }

        public void UpdateConfirmations(int confirmationsCount)
        {
            ConfirmationsCount = confirmationsCount;
        }
    }
}
