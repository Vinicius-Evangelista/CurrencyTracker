namespace Domain.Interfaces;

public interface IExchangeRateService
{
    Task<decimal?> GetRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken );
}
