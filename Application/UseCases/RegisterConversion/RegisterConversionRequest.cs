using Domain.Enums;

namespace Application.UseCases.RegisterConversion;

public class RegisterConversionRequest
{
    public CurrencyCode FromCurrency { get; set; }
    public CurrencyCode ToCurrency { get; set; }
}