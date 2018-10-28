namespace BitcoinApp.Api.Models
{
    public class SendBtcRequest
    {
        public int FromWalletId { get; set; }

        public string ToWalletAddress { get; set; }

        public decimal Amount { get; set; }
    }
}
