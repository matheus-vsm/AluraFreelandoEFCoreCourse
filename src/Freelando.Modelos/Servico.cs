﻿namespace Freelando.Modelo;
public class Servico
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public StatusServico Status { get; set; }
    public Contrato Contrato { get; set; }

    public Servico() { }
    public Servico(Guid id, string? titulo, string? descricao, StatusServico status, Contrato contrato)
    {
        Id = id;
        Titulo = titulo;
        Descricao = descricao;
        Status = status;
        Contrato = contrato;
    }
}
