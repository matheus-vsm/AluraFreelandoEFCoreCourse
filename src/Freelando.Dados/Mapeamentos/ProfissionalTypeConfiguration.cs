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
    internal class ProfissionalTypeConfiguration : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> entity)
        {
            entity.ToTable("TB_Profissionais");
            entity.Property(e => e.Id)
                .HasColumnName("ID_Profissional");
        }
    }
}
