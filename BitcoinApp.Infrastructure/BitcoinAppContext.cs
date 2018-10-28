using System.Collections.Generic;
using BitcoinApp.Domain.InTransactionAggregate;
using BitcoinApp.Domain.OutTransactionAggregate;
using BitcoinApp.Domain.WalletAggregate;
using BitcoinApp.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BitcoinApp.Infrastructure
{
    public class BitcoinAppContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<OutTransaction> OutTransactions { get; set; }

        public DbSet<InTransaction> InTransactions { get; set; }

        public BitcoinAppContext(DbContextOptions<BitcoinAppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WalletConfiguration());
            modelBuilder.ApplyConfiguration(new OutTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new InTransactionConfiguration());

            var wallets = new List<Wallet>();

            var wallet1 = new Wallet("http://127.0.0.1:8332/wallet/wallet1.dat", "password1")
            {
                Id = 1
            };
            wallet1.BalanceIn(0.002m);
            wallets.Add(wallet1);

            var wallet2 = new Wallet("http://127.0.0.1:8332/wallet/wallet2.dat", "password2")
            {
                Id = 2
            };
            wallet2.BalanceIn(0.004m);
            wallets.Add(wallet2);

            var wallet3 = new Wallet("http://127.0.0.1:8332/wallet/wallet3.dat", "password3")
            {
                Id = 3
            };
            wallet3.BalanceIn(0.006m);
            wallets.Add(wallet3);

            modelBuilder.Entity<Wallet>().HasData(wallets.ToArray());

            var inTransactions = new List<InTransaction>();

            var inTransaction = new InTransaction(
                "00b35d6f10f138c6484023cf379a8cfc2da516afd06a1321728ba331e810648f",
                1,
                0.004m,
                "000000000000000000090d549fe271b01dac3b8361ef88d8e5631551519c7cc9");
            inTransaction.Id = 1;
            inTransaction.UpdateConfirmations(2);

            modelBuilder.Entity<InTransaction>().HasData(inTransaction);
        }
    }
}
