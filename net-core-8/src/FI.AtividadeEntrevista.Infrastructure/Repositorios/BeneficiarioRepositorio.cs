using System.Data;
using Dapper;
using FI.AtividadeEntrevista.Domain.Entidades;
using FI.AtividadeEntrevista.Domain.Repositorios;

namespace FI.AtividadeEntrevista.Infrastructure.Repositorios;

public class BeneficiarioRepositorio : IBeneficiarioRepositorio
{
    private readonly FabricaConexao _fabrica;

    public BeneficiarioRepositorio(FabricaConexao fabrica)
    {
        _fabrica = fabrica;
    }

    public async Task<long> IncluirAsync(Beneficiario beneficiario)
    {
        using var conexao = _fabrica.Criar();

        var parametros = new { beneficiario.Cpf, beneficiario.Nome, beneficiario.IdCliente };
        var id = await conexao.ExecuteScalarAsync<object>("FI_SP_IncBenef", parametros, commandType: CommandType.StoredProcedure);

        return Convert.ToInt64(id);
    }

    public async Task AlterarAsync(Beneficiario beneficiario)
    {
        using var conexao = _fabrica.Criar();

        var parametros = new { beneficiario.Cpf, beneficiario.Nome, beneficiario.Id };

        await conexao.ExecuteAsync("FI_SP_AltBenef", parametros, commandType: CommandType.StoredProcedure);
    }

    public async Task ExcluirAsync(long id)
    {
        using var conexao = _fabrica.Criar();

        await conexao.ExecuteAsync("FI_SP_DelBenef", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<IReadOnlyList<Beneficiario>> ListarAsync(long idCliente)
    {
        using var conexao = _fabrica.Criar();

        var lista = await conexao.QueryAsync<Beneficiario>("FI_SP_ConsBenef", new { IdCliente = idCliente }, commandType: CommandType.StoredProcedure);

        return lista.ToList();
    }
}
