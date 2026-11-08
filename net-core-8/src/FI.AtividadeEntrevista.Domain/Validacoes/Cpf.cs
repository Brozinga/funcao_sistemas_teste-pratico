using System.Text.RegularExpressions;

namespace FI.AtividadeEntrevista.Domain.Validacoes;

public static partial class Cpf
{
    private const string CpfProibido = "12345678909";

    [GeneratedRegex(@"^(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})$")]
    private static partial Regex FormatoValido();

    public static bool EhValido(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || !FormatoValido().IsMatch(cpf.Trim()))
            return false;

        var numeros = SomenteNumeros(cpf);

        if (numeros == CpfProibido || numeros.Distinct().Count() == 1)
            return false;

        var primeiroDigito = CalcularDigito(numeros[..9], 10);
        var segundoDigito = CalcularDigito(numeros[..9] + primeiroDigito, 11);

        return numeros.EndsWith($"{primeiroDigito}{segundoDigito}");
    }

    public static string SomenteNumeros(string? cpf)
    {
        return new string((cpf ?? string.Empty).Where(char.IsDigit).ToArray());
    }

    public static string Formatar(string? cpf)
    {
        var numeros = SomenteNumeros(cpf);

        if (numeros.Length != 11)
            return cpf ?? string.Empty;

        return $"{numeros[..3]}.{numeros[3..6]}.{numeros[6..9]}-{numeros[9..]}";
    }

    private static int CalcularDigito(string baseCalculo, int pesoInicial)
    {
        var soma = 0;

        for (var i = 0; i < baseCalculo.Length; i++)
            soma += (baseCalculo[i] - '0') * (pesoInicial - i);

        var resto = soma % 11;

        return resto < 2 ? 0 : 11 - resto;
    }
}
