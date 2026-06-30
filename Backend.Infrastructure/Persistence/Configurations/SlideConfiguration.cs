using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence.Configurations;

internal sealed class SlideConfiguration : IEntityTypeConfiguration<Slide>
{
    public void Configure(EntityTypeBuilder<Slide> builder)
    {
        builder.ToTable("lamina");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Code)
            .HasColumnName("codigo")
            .HasMaxLength(SlideCode.MaxLength)
            .IsRequired()
            .HasConversion(new ValueConverter<SlideCode, string>(
                vo => vo.Value,
                s => SlideCode.Create(s)));

        builder.Property(s => s.Name)
            .HasColumnName("nombre")
            .HasMaxLength(SlideName.MaxLength)
            .IsRequired()
            .HasConversion(new ValueConverter<SlideName, string>(
                vo => vo.Value,
                s => SlideName.Create(s)));

        var textConverter = new ValueConverter<Text?, string?>(
            vo => vo == null ? null : vo.Value,
            s => s == null ? null : Text.Create(s));

        builder.Property(s => s.Description)
            .HasColumnName("descripcion")
            .IsRequired(false)
            .HasConversion(textConverter);

        builder.Property(s => s.Observations)
            .HasColumnName("observaciones")
            .IsRequired(false)
            .HasConversion(textConverter);

        builder.Property(s => s.BoxId)
            .HasColumnName("caja_id")
            .IsRequired();

        builder.HasMany(s => s.Images)
            .WithOne(i => i.Slide)
            .HasForeignKey(i => i.SlideId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
