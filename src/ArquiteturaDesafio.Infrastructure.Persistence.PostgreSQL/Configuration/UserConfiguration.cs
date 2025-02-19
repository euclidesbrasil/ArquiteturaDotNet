using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System.Net;
using System.Reflection.Emit;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Nome da Tabela
            builder.ToTable("Users");

            // Definir Chave Primária
            builder.HasKey(u => u.Id);

            // Configurar Campos
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Firstname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Lastname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Phone)
                .HasMaxLength(20);

            // Enum armazenado como string (opcional)
            builder.Property(u => u.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(u => u.Role)
                .HasConversion<string>()
                .IsRequired();

            // Configuração do Value Object Address (Usando Owned Entity)
            
            builder.OwnsOne(u => u.Address, addresUsr =>
            {
                addresUsr.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("City");

                addresUsr.Property(a => a.Street)
                   .IsRequired()
                   .HasMaxLength(200)
                   .HasColumnName("Street");
                
                addresUsr.Property(a => a.Number);
                
                addresUsr.Property(a => a.ZipCode);

                addresUsr.OwnsOne(a => a.Geolocation, geolocation =>
                {
                    geolocation.Property(g => g.Latitude)
                        .IsRequired()
                        .HasColumnName("Latitude");

                    geolocation.Property(g => g.Longitude)
                        .IsRequired()
                        .HasColumnName("Longitude");
                });





            });
            
            // Índices para melhorar performance em buscas
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();
        }
    }
}
