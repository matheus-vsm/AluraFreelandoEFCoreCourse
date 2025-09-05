using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Freelando.Dados.Mapeamentos
{
    internal class ServicoTypeConfiguration : IEntityTypeConfiguration<Servico>
    {
        public void Configure(EntityTypeBuilder<Servico> entity)
        {
            entity.ToTable("TB_Servicos");
            entity.Property(e => e.Id)
                .HasColumnName("ID_Servico");
            entity.Property(e => e.Descricao)
                .HasColumnType("nvarchar(200)")
                .HasColumnName("DS_Projeto");
            entity.Property(e => e.Status).HasConversion(new EnumToStringConverter<StatusServico>());

            entity
                .HasOne(e => e.Contrato)
                .WithOne(c => c.Servico);

            entity.Property(e => e.ProjetoId).HasColumnName("ID_Projeto");
            entity
                .HasOne(e => e.Projeto)
                .WithOne(p => p.Servico)
                .HasForeignKey<Projeto>(e => e.Id);
        }
    }
}
