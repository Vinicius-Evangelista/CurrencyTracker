namespace Application.UseCases.ViewConversions;

public class ViewConversionsResponse
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public decimal Rate { get; set; }
}