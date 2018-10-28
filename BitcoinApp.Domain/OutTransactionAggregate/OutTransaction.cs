namespace BitcoinApp.Domain.OutTransactionAggregate
{
    public class OutTransaction
    {
        public OutTransaction(string txId, int fromWalletId, string toWalletAddress, decimal amount)
        {
            TxId = txId;
            FromWalletId = fromWalletId;
            ToWalletAddress = toWalletAddress;
            Amount = amount;
        }

        public int Id { get; set; }

        public string TxId { get; }

        public int FromWalletId { get; }

        public string ToWalletAddress { get; }

        public decimal Amount { get; }
    }
}
