using Application.UseCases.RegisterConversion;
using Application.UseCases.ViewConversions;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.ExchangeRateApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CurrencyConversionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<ICurrencyConversionRepository, CurrencyConversionRepository>();
builder.Services.AddScoped<RegisterConversionUseCase>();
builder.Services.AddScoped<ViewConversionUseCase>();


builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

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


app.MapGet("/api/conversions",
    async ([FromQuery] string? searchValue, ViewConversionUseCase useCase) =>
    {
        var result = await useCase.ExecuteAsync(searchValue);

        return Results.Ok(result);
    });

app.Run();