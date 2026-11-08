using System.ComponentModel.DataAnnotations;

namespace FI.AtividadeEntrevista.Web.Models;

public class ClienteModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Campo obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    public string Sobrenome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Digite o CPF no formato 999.999.999-99")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    public string Nacionalidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    public string Cep { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    [MaxLength(2)]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    public string Logradouro { get; set; } = string.Empty;

    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
    public string? Email { get; set; }

    public string? Telefone { get; set; }

    public List<BeneficiarioModel> Beneficiarios { get; set; } = new();
}
