using BitcoinApp.Domain.OutTransactionAggregate;
using BitcoinApp.Domain.WalletAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitcoinApp.Infrastructure.EntityConfigurations
{
    public class OutTransactionConfiguration : IEntityTypeConfiguration<OutTransaction>
    {
        public void Configure(EntityTypeBuilder<OutTransaction> builder)
        {
            builder.ToTable("OutTransactions");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.TxId).IsRequired();
            builder.Property(o => o.FromWalletId).IsRequired();
            builder.Property(o => o.ToWalletAddress).IsRequired();
            builder.Property(o => o.Amount).IsRequired().HasColumnType("decimal(10, 7)");

            builder.HasOne<Wallet>()
                .WithMany()
                .IsRequired()
                .HasForeignKey("FromWalletId");
        }
    }
}
