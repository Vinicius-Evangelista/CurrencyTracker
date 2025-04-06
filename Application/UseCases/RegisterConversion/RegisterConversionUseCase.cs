using Domain.Interfaces;

namespace Application.UseCases.RegisterConversion;

public class RegisterConversionUseCase(
    ICurrencyConversionRepository repository,
    IExchangeRateService exchangeRateService)
{
    public async Task<RegisterConversionResponse> ExecuteAsync(
        RegisterConversionRequest request,
        CancellationToken cancellationToken = default)
    {
        var rate = (decimal)(await exchangeRateService.GetRateAsync(
            request.FromCurrency.ToString(),
            request.ToCurrency.ToString(),
            cancellationToken))!;

        var now = DateTime.UtcNow;

        var conversion = new CurrencyConversion(
            sourceCurrency: new Currency(request.FromCurrency),
            targetCurrency: new Currency(request.ToCurrency),
            rate: rate,
            retrievedAt: DateTimeOffset.Now
        );

        await repository.AddAsync(conversion, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new RegisterConversionResponse
        {
            From = request.FromCurrency,
            To = request.ToCurrency,
            Rate = rate,
            RetrievedAt = now
        };
    }
}