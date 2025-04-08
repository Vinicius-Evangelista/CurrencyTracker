using Domain.Interfaces;

namespace Application.UseCases.ViewConversions;

public class ViewConversionUseCase(ICurrencyConversionRepository repository)
{
    public async Task<IEnumerable<ViewConversionsResponse>> ExecuteAsync(string searchValue)
    {
        var conversions = await repository.GetAllAsync();
        
        if (string.IsNullOrWhiteSpace(searchValue))
            return conversions.Select(c => new ViewConversionsResponse 
            {
                From = c.SourceCurrency.Code.ToString(), 
                To= c.TargetCurrency.Code.ToString(),
                Rate = c.Rate,
            });

        return conversions.Select(c => new ViewConversionsResponse 
        {
            From = c.SourceCurrency.Code.ToString(), 
            To= c.TargetCurrency.Code.ToString(),
            Rate = c.Rate,
        }).Where(x => x.From.Contains(searchValue.ToUpper()) || x.To.Contains(searchValue.ToUpper()));
    }
}