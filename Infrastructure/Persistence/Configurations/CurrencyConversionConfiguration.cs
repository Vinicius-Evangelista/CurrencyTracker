using Domain.Aggregates;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CurrencyConversionConfiguration : IEntityTypeConfiguration<CurrencyConversion>
{
    public void Configure(EntityTypeBuilder<CurrencyConversion> builder)
    {
        builder.ToTable("CurrencyConversions");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever(); 

        builder
            .Property(x => x.SourceCurrency)
            .HasColumnName("source_currency")
            .HasConversion(
                v => v.ToString(),       
                v => new Currency(Enum.Parse<CurrencyCode>(v)) 
            )
            .HasMaxLength(3)
            .IsRequired();

        builder
            .Property(x => x.TargetCurrency)
            .HasColumnName("target_currency")
            .HasConversion(
                v => v.ToString(),
                v => new Currency(Enum.Parse<CurrencyCode>(v))
            )
            .HasMaxLength(3)
            .IsRequired();

        builder
            .Property(x => x.Rate)
            .HasColumnName("rate")
            .IsRequired();

        builder
            .Property(x => x.RetrievedAt)
            .HasColumnName("retrieved_at")
            .IsRequired();
    }
}