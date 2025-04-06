using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class CurrencyConversionDbContext : DbContext
{
    public CurrencyConversionDbContext(DbContextOptions<CurrencyConversionDbContext> options)
        : base(options)
    {
    }

    public DbSet<CurrencyConversion> Conversions => Set<CurrencyConversion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CurrencyConversionDbContext).Assembly);
    }
}