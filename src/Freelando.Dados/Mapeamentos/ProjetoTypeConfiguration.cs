using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos
{
    public class ProjetoTypeConfiguration : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> entity)
        {
            entity.ToTable("TB_Projetos");
            entity.HasKey(e => e.Id).HasName("PK_Projeto");
            entity.Property(e => e.Id)
                .HasColumnName("ID_Projeto");
            entity.Property(e => e.Descricao)
                .HasColumnType("nvarchar(200)")
                .HasColumnName("DS_Projeto");
            entity.Property(e => e.Status)
                .HasConversion(
                    fromObj => fromObj.ToString(), 
                    fromDb => (StatusProjeto)Enum.Parse(typeof (StatusProjeto), fromDb));
            entity
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Projetos)
                .HasForeignKey("ID_Cliente");
            //fromObj => fromObj.ToString() - Quando o EF Core for salvar no banco, ele vai pegar o enum StatusProjeto e converter para string.
            //fromDb => (StatusProjeto)Enum.Parse(typeof(StatusProjeto), fromDb) - Quando o EF Core for ler do banco, ele vai pegar a string e converter para o enum StatusProjeto.
        }
    }
}
