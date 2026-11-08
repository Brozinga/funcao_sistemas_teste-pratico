using FI.AtividadeEntrevista.Domain.Entidades;

namespace FI.AtividadeEntrevista.Web.Models;

public static class ClienteMapeamento
{
    public static Cliente ParaEntidade(this ClienteModel model) => new()
    {
        Id = model.Id,
        Nome = model.Nome,
        Sobrenome = model.Sobrenome,
        Cpf = model.Cpf,
        Nacionalidade = model.Nacionalidade,
        Cep = model.Cep,
        Estado = model.Estado,
        Cidade = model.Cidade,
        Logradouro = model.Logradouro,
        Email = model.Email ?? string.Empty,
        Telefone = model.Telefone ?? string.Empty,
        Beneficiarios = model.Beneficiarios
            .Select(b => new Beneficiario { Id = b.Id, Cpf = b.Cpf, Nome = b.Nome })
            .ToList()
    };

    public static ClienteModel ParaModel(this Cliente cliente) => new()
    {
        Id = cliente.Id,
        Nome = cliente.Nome,
        Sobrenome = cliente.Sobrenome,
        Cpf = Domain.Validacoes.Cpf.Formatar(cliente.Cpf),
        Nacionalidade = cliente.Nacionalidade,
        Cep = cliente.Cep,
        Estado = cliente.Estado,
        Cidade = cliente.Cidade,
        Logradouro = cliente.Logradouro,
        Email = cliente.Email,
        Telefone = cliente.Telefone,
        Beneficiarios = cliente.Beneficiarios
            .Select(b => new BeneficiarioModel { Id = b.Id, Cpf = Domain.Validacoes.Cpf.Formatar(b.Cpf), Nome = b.Nome })
            .ToList()
    };
}
