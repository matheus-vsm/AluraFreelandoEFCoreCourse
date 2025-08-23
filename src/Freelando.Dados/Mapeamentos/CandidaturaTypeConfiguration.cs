using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos
{
    internal class CandidaturaTypeConfiguration : IEntityTypeConfiguration<Candidatura>
    {
        public void Configure(EntityTypeBuilder<Candidatura> entity)
        {
            entity.ToTable("TB_Candidaturas");
            entity.Property(e => e.Id).HasColumnName("ID_Candidatura");
            entity.Property(e => e.ValorProposto).HasColumnName("Valor_Proposto").HasColumnType("decimal(10,2)");
            entity.Property(e => e.DescricaoProposta).HasColumnName("DS_Proposta").HasColumnType("nvarchar(500)");
            entity.Property(e => e.DuracaoProposta).HasColumnName("Duracao_Proposta").HasConversion(new EnumToStringConverter<DuracaoEmDias>());
            entity.Property(e => e.Status).HasConversion(new EnumToStringConverter<StatusCandidatura>());
        }
    }
}
