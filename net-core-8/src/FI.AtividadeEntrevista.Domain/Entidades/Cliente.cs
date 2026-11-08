namespace FI.AtividadeEntrevista.Domain.Entidades;

public class Cliente
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Nacionalidade { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public List<Beneficiario> Beneficiarios { get; set; } = new();
}
