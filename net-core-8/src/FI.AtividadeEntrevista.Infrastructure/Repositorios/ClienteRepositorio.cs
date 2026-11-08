using System.Data;
using Dapper;
using FI.AtividadeEntrevista.Domain.Entidades;
using FI.AtividadeEntrevista.Domain.Repositorios;

namespace FI.AtividadeEntrevista.Infrastructure.Repositorios;

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly FabricaConexao _fabrica;

    public ClienteRepositorio(FabricaConexao fabrica)
    {
        _fabrica = fabrica;
    }

    public async Task<long> IncluirAsync(Cliente cliente)
    {
        using var conexao = _fabrica.Criar();

        var id = await conexao.ExecuteScalarAsync<object>("FI_SP_IncClienteV2", ParametrosDe(cliente), commandType: CommandType.StoredProcedure);

        return Convert.ToInt64(id);
    }

    public async Task AlterarAsync(Cliente cliente)
    {
        using var conexao = _fabrica.Criar();

        var parametros = ParametrosDe(cliente);
        parametros.Add("Id", cliente.Id);

        await conexao.ExecuteAsync("FI_SP_AltCliente", parametros, commandType: CommandType.StoredProcedure);
    }

    public async Task<Cliente?> ConsultarAsync(long id)
    {
        using var conexao = _fabrica.Criar();

        return await conexao.QueryFirstOrDefaultAsync<Cliente>("FI_SP_ConsCliente", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<(IReadOnlyList<Cliente> Registros, int Total)> PesquisarAsync(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente)
    {
        using var conexao = _fabrica.Criar();

        var parametros = new { iniciarEm, quantidade, campoOrdenacao, crescente };

        using var resultado = await conexao.QueryMultipleAsync("FI_SP_PesqCliente", parametros, commandType: CommandType.StoredProcedure);

        var registros = (await resultado.ReadAsync<Cliente>()).ToList();
        var total = await resultado.ReadFirstAsync<int>();

        return (registros, total);
    }

    public async Task<bool> CpfJaCadastradoAsync(string cpf, long idCliente)
    {
        using var conexao = _fabrica.Criar();

        var existe = await conexao.QueryFirstOrDefaultAsync<int?>("FI_SP_VerificaCliente", new { Cpf = cpf, Id = idCliente }, commandType: CommandType.StoredProcedure);

        return existe.HasValue;
    }

    private static DynamicParameters ParametrosDe(Cliente cliente)
    {
        var parametros = new DynamicParameters();
        parametros.Add("Nome", cliente.Nome);
        parametros.Add("Sobrenome", cliente.Sobrenome);
        parametros.Add("Nacionalidade", cliente.Nacionalidade);
        parametros.Add("Cep", cliente.Cep);
        parametros.Add("Estado", cliente.Estado);
        parametros.Add("Cidade", cliente.Cidade);
        parametros.Add("Logradouro", cliente.Logradouro);
        parametros.Add("Email", cliente.Email);
        parametros.Add("Telefone", cliente.Telefone);
        parametros.Add("Cpf", cliente.Cpf);

        return parametros;
    }
}
