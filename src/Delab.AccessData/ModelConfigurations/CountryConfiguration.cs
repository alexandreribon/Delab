using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfigurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

        builder.Property(c => c.CodPhone)
                .HasColumnType("varchar(5)");

        builder.HasIndex(c => c.Name).IsUnique();

        //Relacionamento: 1 : N => Country : State
        builder.HasMany(c => c.States)
            .WithOne(s => s.Country)
            .HasForeignKey(s => s.CountryId);
    }
}
