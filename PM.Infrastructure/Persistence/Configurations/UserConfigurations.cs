using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PM.Domain.CountryAggregate.Entities;
using PM.Domain.CountryAggregate.ValueObjects;
using PM.Domain.UserAggregate;
using PM.Domain.UserAggregate.ValueObjects;

namespace PM.Infrastructure.Persistence.Configurations;

public sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("Users");

        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder
            .Property(u => u.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.HasIndex(m => m.Email)
            .HasDatabaseName("IX_User_Email")
            .IsUnique();

        builder
            .Property(u => u.PasswordHash)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(u => u.CreatedDateUtc)
            .IsRequired();


        builder
            .Property(d => d.ProvinceId)
            .HasConversion(
                id => id.Value,
                value => ProvinceId.Create(value));

        builder.HasIndex(u => u.ProvinceId)
            .HasDatabaseName("IX_User_Province");

        builder.HasOne<Province>()
            .WithMany()
            .HasForeignKey(o => o.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.HasOne(u => u.)
        //    .WithMany()
        //    .HasForeignKey(u => u.ProvinceId
        //    .OnDelete(DeleteBehavior.Restrict);


    }
}