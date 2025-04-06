using Domain.ValueObjects;

namespace Domain.Aggregates;

public class CurrencyConversion
{
    public Guid Id { get; set;  }
    public Currency SourceCurrency { get; set;  }
    public Currency TargetCurrency { get; set;  }
    public decimal Rate { get; set;  }
    public DateTimeOffset RetrievedAt { get; set; }
    
    public  CurrencyConversion() { }

    public CurrencyConversion(Currency sourceCurrency, Currency targetCurrency, decimal rate, DateTimeOffset retrievedAt)
    {
        Id = Guid.NewGuid();
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Rate = rate;
        RetrievedAt = retrievedAt;
    }

    public void UpdateRate(decimal newRate, DateTime retrievedAt)
    {
        Rate = newRate;
        RetrievedAt = retrievedAt;
    }
}