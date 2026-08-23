using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CuentaConfiguracion : IEntityTypeConfiguration<Cuenta>
{
    public void Configure(EntityTypeBuilder<Cuenta> builder)
    {
        builder.ToTable("cuentas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.NumeroCuenta)
            .HasColumnName("numero_cuenta")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.NumeroCuenta)
            .IsUnique();

        builder.Property(x => x.Titular)
            .HasColumnName("titular")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Saldo)
            .HasColumnName("saldo")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasColumnName("estado")
            .IsRequired();

        builder.Property(x => x.FechaCreacionUtc)
            .HasColumnName("fecha_creacion_utc")
            .IsRequired();
    }
}
