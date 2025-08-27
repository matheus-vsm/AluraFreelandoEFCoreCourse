using Freelando.Modelos;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freelando.Modelo;
public class Especialidade
{
    public Guid Id { get; set; }
    public string? Descricao { get; set; }
    public ICollection<Projeto> Projetos { get; set; }
    public ICollection<ProjetoEspecialidade> ProjetosEspecialidade { get; } = [];
    // Como a lista já está no banco de dados, não precisa do metodo set

    public Especialidade() { }
    public Especialidade(Guid id, string? descricao, ICollection<Projeto> projetos)
    {
        Id = id;
        Descricao = descricao;
        Projetos = projetos;
    }
}
