using BitcoinApp.Domain.WalletAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitcoinApp.Infrastructure.EntityConfigurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Balance).IsRequired().HasColumnType("decimal(10, 7)");
            builder.Property(o => o.Address).IsRequired();
            builder.Property(o => o.Password).IsRequired();
        }
    }
}
