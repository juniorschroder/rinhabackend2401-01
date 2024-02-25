using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RinhaBackend.Domain.Entities;

namespace RinhaBackend.Infrastructure.Configurations;

public class ClienteConfigurationMap : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("cliente");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("id").IsRequired();
        builder.Property(p => p.Limite).HasColumnName("limite").IsRequired();
    }
}