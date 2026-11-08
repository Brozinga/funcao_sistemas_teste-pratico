using FI.AtividadeEntrevista.Domain.Repositorios;
using FI.AtividadeEntrevista.Infrastructure.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FI.AtividadeEntrevista.Infrastructure;

public static class InjecaoDependencia
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection servicos, IConfiguration configuracao)
    {
        // No Windows usa o LocalDB (arquivo .mdf em App_Data); em Linux/macOS usa o SQL Server do Docker.
        var stringConexao = (OperatingSystem.IsWindows() ? configuracao["BancoDeDados:ConnectionStringLocalDb"] : null)
            ?? configuracao.GetConnectionString("BancoDeDados")
            ?? throw new InvalidOperationException("A connection string 'BancoDeDados' não foi encontrada no appsettings.json.");

        servicos.AddSingleton(new FabricaConexao(stringConexao));
        servicos.AddScoped<IClienteRepositorio, ClienteRepositorio>();
        servicos.AddScoped<IBeneficiarioRepositorio, BeneficiarioRepositorio>();

        return servicos;
    }
}
