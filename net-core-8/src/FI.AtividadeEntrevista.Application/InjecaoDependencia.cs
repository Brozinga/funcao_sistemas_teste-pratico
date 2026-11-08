using FI.AtividadeEntrevista.Application.Servicos;
using Microsoft.Extensions.DependencyInjection;

namespace FI.AtividadeEntrevista.Application;

public static class InjecaoDependencia
{
    public static IServiceCollection AddApplication(this IServiceCollection servicos)
    {
        servicos.AddScoped<IClienteServico, ClienteServico>();

        return servicos;
    }
}
