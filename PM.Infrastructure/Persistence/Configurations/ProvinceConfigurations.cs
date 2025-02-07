using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PM.Domain.CountryAggregate;
using PM.Domain.CountryAggregate.Entities;
using PM.Domain.CountryAggregate.ValueObjects;
using PM.Domain.UserAggregate;

namespace PM.Infrastructure.Persistence.Configurations;

public sealed class ProvinceConfigurations : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        ConfigureProvincesTable(builder);
        //ConfigureProvincesTable(builder);
    }

    private void ConfigureProvincesTable(EntityTypeBuilder<Province> builder)
    {
        builder
            .ToTable("Provinces");

        builder
            .HasKey(m => m.Id);

        builder
            .Property(m => m.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ProvinceId.Create(value));

        builder
            .Property(m => m.Name)
            .HasMaxLength(512)
            .IsRequired();
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