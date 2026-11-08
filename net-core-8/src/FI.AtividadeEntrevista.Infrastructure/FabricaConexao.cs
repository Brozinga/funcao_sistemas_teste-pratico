using System.Data;
using Microsoft.Data.SqlClient;

namespace FI.AtividadeEntrevista.Infrastructure;

public class FabricaConexao
{
    private readonly string _stringConexao;

    public FabricaConexao(string stringConexao)
    {
        _stringConexao = stringConexao;
    }

    public IDbConnection Criar() => new SqlConnection(_stringConexao);
}
