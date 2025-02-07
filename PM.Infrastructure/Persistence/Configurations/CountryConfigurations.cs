using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PM.Domain.CountryAggregate;
using PM.Domain.CountryAggregate.ValueObjects;
using PM.Domain.UserAggregate;

namespace PM.Infrastructure.Persistence.Configurations;

public sealed class CountryConfigurations : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        ConfigureCountriesTable(builder);
        //ConfigureProvincesTable(builder);
    }

    private void ConfigureCountriesTable(EntityTypeBuilder<Country> builder)
    {
        builder
            .ToTable("Countries");

        builder
            .HasKey(m => m.Id);

        builder
            .Property(m => m.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => CountryId.Create(value));

        builder
            .Property(m => m.Name)
            .HasMaxLength(512)
            .IsRequired();

        builder.HasMany(b => b.Provinces)
            .WithOne();
    }

    private void ConfigureProvincesTable(EntityTypeBuilder<Country> builder)
    {
        builder.OwnsMany(m => m.Provinces, sectionBuilder =>
        {
            sectionBuilder.ToTable("Provinces");

            sectionBuilder
                .WithOwner()
                .HasForeignKey("CountryId");

            sectionBuilder.HasKey("Id");
            sectionBuilder.HasIndex("CountryId", "Id")
                .HasDatabaseName("IX_Province_Country");
            //sectionBuilder.HasKey("Id", "MenuId");

            sectionBuilder.Property(s => s.Id)
                .HasColumnName("ProvinceId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ProvinceId.Create(value));

            sectionBuilder
                .Property(s => s.Name)
                .HasMaxLength(512);
        });
    }
}