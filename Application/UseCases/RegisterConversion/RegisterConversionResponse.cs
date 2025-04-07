using Domain.Enums;

namespace Application.UseCases.RegisterConversion;

public class RegisterConversionResponse
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Rate { get; set; }
    public DateTime RetrievedAt { get; set; }
}