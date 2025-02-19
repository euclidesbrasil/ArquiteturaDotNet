using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System.Net;
using System.Reflection.Emit;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Type)
                .HasConversion<int>() // Armazena como INT no banco
                .IsRequired();

            builder.OwnsOne(t => t.Amount, amount =>
            {
                amount.Property(a => a.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(18, 2) // Define precisão decimal
                    .IsRequired();
            });

            builder.Property(t => t.Date)
                .HasColumnName("date")
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(t => t.Consolidated)
                .HasColumnName("consolidated")
                .IsRequired();
        }
    }
}
