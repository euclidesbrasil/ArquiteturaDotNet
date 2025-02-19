using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ArquiteturaDesafio.Infrastructure.Persistence.PostgreSQL.Context;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<DailyBalance> DailyBalances { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new DailyBalanceConfiguration());
    }
}

