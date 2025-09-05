﻿using Freelando.Dados.Interceptor;
using Freelando.Modelo;
using Freelando.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Freelando.Dados
{
    public class FreelandoContext : DbContext
    {
        public DbSet<Candidatura> Candidaturas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<ProjetoEspecialidade> ProjetosEspecialidades { get; set; }
        public DbSet<ProfissionalEspecialidade> ProfissionalEspecialidades { get; set; }

        private readonly IConfiguration _configuration;
        public FreelandoContext(DbContextOptions<FreelandoContext> options) : base(options) { }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ConnectionStrings:DefaultConnection"));
            }
            optionsBuilder.AddInterceptors(new CommandInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FreelandoContext).Assembly);
        }
    }
}
