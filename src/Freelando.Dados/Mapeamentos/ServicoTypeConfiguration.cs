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
    internal class ServicoTypeConfiguration : IEntityTypeConfiguration<Servico>
    {
        public void Configure(EntityTypeBuilder<Servico> entity)
        {
            entity.ToTable("TB_Servicos");
            entity.Property(e => e.Id)
                .HasColumnName("ID_Servico");
            entity.Property(e => e.Descricao)
                .HasColumnType("nvarchar(200)")
                .HasColumnName("DS_Servico");
            entity.Property(e => e.Status).HasConversion(new EnumToStringConverter<StatusServico>());
        }
    }
}
