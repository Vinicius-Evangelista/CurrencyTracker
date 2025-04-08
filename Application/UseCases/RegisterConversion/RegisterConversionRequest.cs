using Domain.Enums;

namespace Application.UseCases.RegisterConversion;

public class RegisterConversionRequest
{
    public string FromCurrency { get; set; }
    public string ToCurrency { get; set; }
}