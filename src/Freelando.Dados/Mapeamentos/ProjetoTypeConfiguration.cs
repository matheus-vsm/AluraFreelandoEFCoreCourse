using Freelando.Modelo;
using Freelando.Modelos;
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
                    fromDb => (StatusProjeto)Enum.Parse(typeof(StatusProjeto), fromDb));
            entity
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Projetos)
                .HasForeignKey("ID_Cliente");

            entity
                .HasMany(e => e.Especialidades)
                .WithMany(e => e.Projetos)
                .UsingEntity<ProjetoEspecialidade>(
                    espe => espe
                        .HasOne<Especialidade>(e => e.Especialidade)
                        .WithMany(e => e.ProjetosEspecialidade)
                        .HasForeignKey(e => e.EspecialidadeId),
                    proj => proj
                        .HasOne<Projeto>(e => e.Projeto)
                        .WithMany(e => e.ProjetosEspecialidade)
                        .HasForeignKey(e => e.ProjetoId)
                    );
            //configuração alternativa para muitos para muitos sem a entidade de junção
            //entity
            //    .HasMany(p => p.Especialidades)
            //    .WithMany(e => e.Projetos)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "TB_Especialidade_Projeto", // Nome da tabela de junção
            //        j => j
            //            .HasOne<Especialidade>()
            //            .WithMany()
            //            .HasForeignKey("Id_Especialidade")
            //            .HasConstraintName("FK_Especialidade_Projeto_Especialidade"),
            //        j => j
            //            .HasOne<Projeto>()
            //            .WithMany()
            //            .HasForeignKey("Id_Projeto")
            //            .HasConstraintName("FK_Especialidade_Projeto_Projeto")
            //    );
            //fromObj => fromObj.ToString() - Quando o EF Core for salvar no banco, ele vai pegar o enum StatusProjeto e converter para string.
            //fromDb => (StatusProjeto)Enum.Parse(typeof(StatusProjeto), fromDb) - Quando o EF Core for ler do banco, ele vai pegar a string e converter para o enum StatusProjeto.
        }
    }
}
