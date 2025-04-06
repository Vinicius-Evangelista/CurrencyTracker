using Domain.Enums;

namespace Application.UseCases.RegisterConversion;

public class RegisterConversionResponse
{
    public CurrencyCode From { get; set; }
    public CurrencyCode To { get; set; }
    public decimal Rate { get; set; }
    public DateTime RetrievedAt { get; set; }
}