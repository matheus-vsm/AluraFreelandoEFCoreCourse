﻿namespace Freelando.Modelo;
public class Cliente
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }

    public Cliente(Guid id, string? nome, string? cpf, string? email, string? telefone)
    {
        Id = id;
        Nome = nome;
        Cpf = cpf;
        Email = email;
        Telefone = telefone;
    }
}
