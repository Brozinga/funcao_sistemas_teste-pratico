using FI.AtividadeEntrevista.Domain.Entidades;

namespace FI.AtividadeEntrevista.Application.Servicos;

public interface IClienteServico
{
    Task<Resultado> IncluirAsync(Cliente cliente);
    Task<Resultado> AlterarAsync(Cliente cliente);
    Task<Cliente?> ConsultarAsync(long id);
    Task<PaginaClientes> PesquisarAsync(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente);
}
