using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RinhaBackend.Domain.Entities;

namespace RinhaBackend.Infrastructure.Configurations;

public class TransacaoConfigurationMap : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("transacao");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id").IsRequired();
        builder.Property(p => p.Valor).HasColumnName("valor").IsRequired();
        builder.Property(p => p.ClienteId).HasColumnName("cliente_id").IsRequired();
        builder.Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(10).IsRequired();
        builder.Property(p => p.Tipo)
            .HasColumnName("tipo_transacao")
            .IsRequired();
        builder.Property(p => p.RealizadaEm)
            .HasColumnName("realizada_em")
            .HasDefaultValue(DateTime.Now)
            .IsRequired();
        builder.Property(p => p.Saldo).HasColumnName("saldo").IsRequired(false);
    }
}