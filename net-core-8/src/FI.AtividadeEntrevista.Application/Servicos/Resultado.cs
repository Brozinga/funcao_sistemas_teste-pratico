namespace FI.AtividadeEntrevista.Application.Servicos;

public class Resultado
{
    public bool Sucesso => Erro is null;
    public string? Erro { get; }

    private Resultado(string? erro)
    {
        Erro = erro;
    }

    public static Resultado Ok() => new(null);

    public static Resultado Falha(string erro) => new(erro);
}
