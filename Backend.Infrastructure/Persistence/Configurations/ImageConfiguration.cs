using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.Entities;
using UCR.EB.BioMicroscopeAdmin.Backend.Domain.ValueObjects;

namespace UCR.EB.BioMicroscopeAdmin.Backend.Infrastructure.Persistence.Configurations;

internal sealed class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("imagen");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(i => i.FileUrl)
            .HasColumnName("url_archivo")
            .HasMaxLength(ImageUrl.MaxLength)
            .IsRequired()
            .HasConversion(new ValueConverter<ImageUrl, string>(
                vo => vo.Value,
                s => ImageUrl.Create(s)));

        builder.Property(i => i.FileName)
            .HasColumnName("nombre_archivo")
            .HasMaxLength(ImageFileName.MaxLength)
            .IsRequired()
            .HasConversion(new ValueConverter<ImageFileName, string>(
                vo => vo.Value,
                s => ImageFileName.Create(s)));

        builder.Property(i => i.Order)
            .HasColumnName("orden")
            .IsRequired()
            .HasConversion(new ValueConverter<ImageOrder, int>(
                vo => vo.Value,
                i => ImageOrder.Create(i)));

        builder.Property(i => i.UploadedAt)
            .HasColumnName("fecha_carga")
            .IsRequired();

        builder.Property(i => i.SlideId)
            .HasColumnName("lamina_id")
            .IsRequired();

        builder.ToTable("imagen", t => t.HasCheckConstraint("CK_imagen_orden", "orden >= 1 AND orden <= 6"));

        builder.HasIndex(i => i.SlideId);
    }
}
