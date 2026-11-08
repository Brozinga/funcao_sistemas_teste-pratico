using System.ComponentModel.DataAnnotations;

namespace FI.AtividadeEntrevista.Web.Models;

public class BeneficiarioModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Campo obrigatório.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Digite o CPF do beneficiário no formato 999.999.999-99")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obrigatório.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do beneficiário deve ter no mínimo 3 letras")]
    public string Nome { get; set; } = string.Empty;
}
