namespace BitcoinApp.Domain.WalletAggregate
{
    public class Wallet
    {
        public Wallet(string address, string password)
        {
            Address = address;
            Password = password;
        }

        public int Id { get; set; }

        public decimal Balance { get; private set; }

        public void BalanceIn(decimal value)
        {
            Balance += value;
        }

        public bool BalanceOut(decimal value)
        {
            if (Balance <= 0 || Balance < value)
            {
                return false;
            }

            Balance -= value;

            return true;
        }

        public string Password { get; }

        public string Address { get; }

    }
}
