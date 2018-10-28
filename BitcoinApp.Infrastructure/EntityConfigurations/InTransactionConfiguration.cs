using BitcoinApp.Domain.InTransactionAggregate;
using BitcoinApp.Domain.WalletAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitcoinApp.Infrastructure.EntityConfigurations
{
    public class InTransactionConfiguration : IEntityTypeConfiguration<InTransaction>
    {
        public void Configure(EntityTypeBuilder<InTransaction> builder)
        {
            builder.ToTable("InTransactions");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.TxId).IsRequired();
            builder.Property(o => o.Amount).IsRequired().HasColumnType("decimal(10, 7)");
            builder.Property(o => o.ConfirmationsCount).IsRequired();
            builder.Property(o => o.ToWalletId).IsRequired();
            builder.Property(o => o.BlockHash).IsRequired();

            builder.HasOne<Wallet>()
                .WithMany()
                .IsRequired()
                .HasForeignKey("ToWalletId");

            builder.HasIndex(o => o.ConfirmationsCount);
        }
    }
}
