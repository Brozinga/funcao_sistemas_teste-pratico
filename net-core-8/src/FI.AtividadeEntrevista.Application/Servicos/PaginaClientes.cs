using FI.AtividadeEntrevista.Domain.Entidades;

namespace FI.AtividadeEntrevista.Application.Servicos;

public record PaginaClientes(IReadOnlyList<Cliente> Registros, int Total);
