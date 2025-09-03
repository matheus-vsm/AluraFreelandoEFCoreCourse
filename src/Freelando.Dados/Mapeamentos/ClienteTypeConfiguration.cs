﻿using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Dados.Mapeamentos
{
    internal class ClienteTypeConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> entity)
        {
            entity.ToTable("TB_Clientes");
            entity.Property(e => e.Id)
                .HasColumnName("ID_Cliente");
            entity.HasIndex(e => e.Email).IsUnique();

            entity.OwnsOne(e => e.Endereco, endereco =>
            {
                endereco.Property(e => e.Cep).HasColumnName("Cep");
                endereco.Property(e => e.Cidade).HasColumnName("Cidade");
                endereco.Property(e => e.Estado).HasColumnName("Estado");
                endereco.Property(e => e.Bairro).HasColumnName("Bairro");
                endereco.Property(e => e.Logradouro).HasColumnName("Logradouro");
                endereco.Property(e => e.Numero).HasColumnName("Numero");
                endereco.Property(e => e.Complemento).HasColumnName("Complemento");
            });
        }
    }
}
