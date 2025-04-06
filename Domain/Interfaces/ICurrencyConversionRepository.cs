using Domain.Aggregates;

namespace Domain.Interfaces;

public interface ICurrencyConversionRepository
{
    Task AddAsync(CurrencyConversion conversion, CancellationToken cancellationToken = default);

    Task UpdateAsync(CurrencyConversion conversion, CancellationToken cancellationToken = default);

    Task<CurrencyConversion?> GetLatestAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}