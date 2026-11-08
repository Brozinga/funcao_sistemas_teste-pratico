namespace FI.AtividadeEntrevista.Domain.Entidades;

public class Beneficiario
{
    public long Id { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public long IdCliente { get; set; }
}
