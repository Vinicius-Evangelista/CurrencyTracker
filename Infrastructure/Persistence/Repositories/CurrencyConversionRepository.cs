using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CurrencyConversionRepository : ICurrencyConversionRepository
{
    private readonly CurrencyConversionDbContext _context;

    public CurrencyConversionRepository(CurrencyConversionDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CurrencyConversion conversion, CancellationToken cancellationToken = default)
    {
        await _context.Conversions.AddAsync(conversion, cancellationToken);
    }

    public async Task<CurrencyConversion?> GetLatestAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken = default)
    {
        return await _context.Conversions
            .Where(x => x.SourceCurrency.Code.ToString() == fromCurrency && 
                        x.TargetCurrency.Code.ToString() == toCurrency)
            .OrderByDescending(x => x.RetrievedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task UpdateAsync(CurrencyConversion conversion, CancellationToken cancellationToken = default)
    {
        _context.Conversions.Update(conversion);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}