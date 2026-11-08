using FI.AtividadeEntrevista.Domain.Entidades;

namespace FI.AtividadeEntrevista.Domain.Repositorios;

public interface IBeneficiarioRepositorio
{
    Task<long> IncluirAsync(Beneficiario beneficiario);
    Task AlterarAsync(Beneficiario beneficiario);
    Task ExcluirAsync(long id);
    Task<IReadOnlyList<Beneficiario>> ListarAsync(long idCliente);
}
