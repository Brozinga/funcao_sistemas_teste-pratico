using FI.AtividadeEntrevista.Domain.Validacoes;

namespace FI.AtividadeEntrevista.Tests;

public class CpfTestes
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void EhValido_CpfCorreto_RetornaVerdadeiro(string cpf)
    {
        Assert.True(Cpf.EhValido(cpf));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("529.982.247-24")]
    [InlineData("111.111.111-11")]
    [InlineData("123.456.789-09")]
    [InlineData("5299822472")]
    [InlineData("529.982.24725")]
    [InlineData("abc.def.ghi-jk")]
    public void EhValido_CpfIncorreto_RetornaFalso(string? cpf)
    {
        Assert.False(Cpf.EhValido(cpf));
    }

    [Fact]
    public void SomenteNumeros_RemoveFormatacao()
    {
        Assert.Equal("52998224725", Cpf.SomenteNumeros("529.982.247-25"));
    }

    [Fact]
    public void Formatar_AplicaMascara()
    {
        Assert.Equal("529.982.247-25", Cpf.Formatar("52998224725"));
    }

    [Fact]
    public void Formatar_ValorIncompleto_MantemOriginal()
    {
        Assert.Equal("123", Cpf.Formatar("123"));
    }
}
