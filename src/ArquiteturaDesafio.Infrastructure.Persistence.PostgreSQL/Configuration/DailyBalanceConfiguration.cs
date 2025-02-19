using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System.Net;
using System.Reflection.Emit;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Configuration
{
    public class DailyBalanceConfiguration : IEntityTypeConfiguration<DailyBalance>
    {
        public void Configure(EntityTypeBuilder<DailyBalance> builder)
        {
            builder.ToTable("daily_balances");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Date)
                .HasColumnName("date")
                .IsRequired();

            builder.OwnsOne(d => d.InitialBalance, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("initial_balance")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            builder.OwnsOne(d => d.FinalBalance, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("final_balance")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            builder.OwnsOne(d => d.TotalCredits, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("total_credits")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            builder.OwnsOne(d => d.TotalDebits, balance =>
            {
                balance.Property(b => b.Amount)
                    .HasColumnName("total_debits")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            builder.Property(d => d.TransactionCount)
                .HasColumnName("transaction_count")
                .IsRequired();
        }
    }
}
