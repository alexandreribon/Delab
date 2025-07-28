using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfigurations;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("States");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
                .IsRequired()
                .HasColumnType("varchar(150)");

        builder.HasIndex(s => new { s.Name, s.CountryId }).IsUnique();

        //Relacionamento: 1 : N => State : City
        builder.HasMany(s => s.Cities)
            .WithOne(c => c.State)
            .HasForeignKey(c => c.StateId);
    }
}
