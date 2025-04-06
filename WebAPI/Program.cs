using Application.UseCases.RegisterConversion;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.ExchangeRateApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core + MSSQL
builder.Services.AddDbContext<CurrencyConversionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<ICurrencyConversionRepository, CurrencyConversionRepository>();
builder.Services.AddScoped<RegisterConversionUseCase>();

builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/currency-conversions", async (
    RegisterConversionRequest request,
    RegisterConversionUseCase useCase,
    CancellationToken cancellationToken) =>
{
    var response = await useCase.ExecuteAsync(request, cancellationToken);
    return Results.Ok(response);
});

app.Run();