using Domain.Interfaces;
namespace Application.UseCases.UpdateConversion;

public class UpdateExchangeRatesUseCase(
    ICurrencyConversionRepository repository,
    IExchangeRateService exchangeRateService)
{
    public async Task ExecuteAsync(CancellationToken cts = default)
    {
        var conversions = await repository.GetAllAsync(cts);

        foreach (var conversion in conversions)
        {
            var newRate = await exchangeRateService.GetRateAsync(conversion.SourceCurrency.ToString(),
                conversion.TargetCurrency.ToString(), cts);

            conversion.Rate = (decimal)newRate!;
            conversion.RetrievedAt = DateTimeOffset.Now;
        }

        await repository.SaveChangesAsync(cts);
    }
}