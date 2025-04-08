using Application.UseCases.UpdateConversion;
using Domain.Interfaces;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services.ExchangeRateApi;
using Microsoft.EntityFrameworkCore;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        services.AddScoped<ICurrencyConversionRepository, CurrencyConversionRepository>();
        
        services.AddDbContext<CurrencyConversionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddHangfire(config =>
            config.UseSqlServerStorage(configuration.GetConnectionString("Default"),
            new SqlServerStorageOptions()
            {
                PrepareSchemaIfNecessary = true
            })
        );

        services.AddHangfireServer();

        services.AddTransient<UpdateExchangeRatesUseCase>();
        services.AddHostedService<Worker.Worker>();
    })
    .Build()
    .Run();