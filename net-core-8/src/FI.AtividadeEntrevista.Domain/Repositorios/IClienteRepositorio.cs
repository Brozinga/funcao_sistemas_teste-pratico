using FI.AtividadeEntrevista.Domain.Entidades;

namespace FI.AtividadeEntrevista.Domain.Repositorios;

public interface IClienteRepositorio
{
    Task<long> IncluirAsync(Cliente cliente);
    Task AlterarAsync(Cliente cliente);
    Task<Cliente?> ConsultarAsync(long id);
    Task<(IReadOnlyList<Cliente> Registros, int Total)> PesquisarAsync(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente);
    Task<bool> CpfJaCadastradoAsync(string cpf, long idCliente);
}
