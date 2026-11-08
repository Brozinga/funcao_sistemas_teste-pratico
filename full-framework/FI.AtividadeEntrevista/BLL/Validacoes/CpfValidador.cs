using System.Text.RegularExpressions;

//Codigo reutilizado do projeto - rdakar / Validação de CPF e CNPJ - C#:
//https://gist.github.com/rdakar/dba890b5e2cbdeb7c62c0dee9f627a7f

namespace FI.AtividadeEntrevista.BLL
{
    public class CpfValidador
    {
        public static bool EhValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Trim();

            if (!Regex.IsMatch(cpf, @"^(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})$"))
                return false;

            if (SomenteNumeros(cpf) == "12345678909")
                return false;

            return IsCpf(cpf);
        }

        public static string SomenteNumeros(string cpf)
        {
            if (cpf == null)
                return null;

            return cpf.Replace(".", "").Replace("-", "").Trim();
        }

        public static string Formatar(string cpf)
        {
            string numeros = SomenteNumeros(cpf);

            if (numeros == null || numeros.Length != 11)
                return cpf;

            return numeros.Substring(0, 3) + "." + numeros.Substring(3, 3) + "." + numeros.Substring(6, 3) + "-" + numeros.Substring(9, 2);
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
