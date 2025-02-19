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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}

