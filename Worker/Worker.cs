using Application.UseCases.UpdateConversion;
using Hangfire;

namespace Worker;

public class Worker(IRecurringJobManager jobManager) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        jobManager.AddOrUpdate<UpdateExchangeRatesUseCase>(
            "update-currency-rates",
            job => job.ExecuteAsync(default), 
            Cron.Hourly
        );

        return Task.CompletedTask;
    }
}