using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence.Configurations;

internal sealed class BoxConfiguration : IEntityTypeConfiguration<Box>
{
    public void Configure(EntityTypeBuilder<Box> builder)
    {
        builder.ToTable("caja");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Number)
            .HasColumnName("numero")
            .IsRequired()
            .HasConversion(new ValueConverter<BoxNumber, int>(
                vo => vo.Value,
                i => BoxNumber.Create(i)));

        builder.Property(b => b.Name)
            .HasColumnName("nombre")
            .HasMaxLength(BoxName.MaxLength)
            .IsRequired()
            .HasConversion(new ValueConverter<BoxName, string>(
                vo => vo.Value,
                s => BoxName.Create(s)));

        builder.Property(b => b.GroupId)
            .HasColumnName("grupo_id")
            .IsRequired();

        builder.HasMany(b => b.Slides)
            .WithOne(s => s.Box)
            .HasForeignKey(s => s.BoxId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
