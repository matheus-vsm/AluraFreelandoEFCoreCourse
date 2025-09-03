﻿using Freelando.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelando.Modelo;
public class Projeto
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public  string? Descricao { get; set; }
    public StatusProjeto Status { get; set; }
    public Cliente? Cliente { get; set; }
    public ICollection<Especialidade> Especialidades { get; set; }
    public ICollection<ProjetoEspecialidade> ProjetosEspecialidade { get; } = [];
    public Servico Servico { get; set; }
    public Vigencia Vigencia { get; set; }

    public Projeto() { }
    public Projeto(Guid id, string? titulo, string? descricao, StatusProjeto status, Cliente cliente, ICollection<Especialidade> especialidades, Servico servico)
    {
        Id = id;
        Titulo = titulo;
        Descricao = descricao;
        Status = status;
        Cliente = cliente;
        Especialidades = especialidades;
        Servico = servico;
    }
}
