using System.Net.Http.Json;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.ExchangeRateApi;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(HttpClient httpClient, ILogger<ExchangeRateService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<decimal?> GetRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fromCurrency))
            throw new ArgumentException("From currency must be provided", nameof(fromCurrency));

        if (string.IsNullOrWhiteSpace(toCurrency))
            throw new ArgumentException("To currency must be provided", nameof(toCurrency));

        var endpoint = $"https://api.exchangerate.host/convert?from={fromCurrency}&to={toCurrency}&access_key=beefe3ae4258c51a75cbf2e1131249f8&amount=1";

        try
        {
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API call failed with status code {StatusCode} for request {Endpoint}", response.StatusCode, endpoint);
                return null;
            }

            var content = await response.Content.ReadFromJsonAsync<ExchangeRateResponse.ExchangeRateApiResponse>(cancellationToken: cancellationToken);

            if (content is null)
            {
                _logger.LogWarning("API response was not successful or content is missing for request {Endpoint}", endpoint);
                return null;
            }

            return content.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching exchange rate from {From} to {To}", fromCurrency, toCurrency);
            return null;
        }
    }
}
