using FI.AtividadeEntrevista.Application.Servicos;
using FI.AtividadeEntrevista.Domain.Entidades;
using FI.AtividadeEntrevista.Domain.Repositorios;
using NSubstitute;

namespace FI.AtividadeEntrevista.Tests;

public class ClienteServicoTestes
{
    private const string CpfCliente = "529.982.247-25";
    private const string CpfBeneficiarioA = "111.444.777-35";
    private const string CpfBeneficiarioB = "390.533.447-05";

    private readonly IClienteRepositorio _clientes = Substitute.For<IClienteRepositorio>();
    private readonly IBeneficiarioRepositorio _beneficiarios = Substitute.For<IBeneficiarioRepositorio>();
    private readonly ClienteServico _servico;

    public ClienteServicoTestes()
    {
        _beneficiarios.ListarAsync(Arg.Any<long>()).Returns(new List<Beneficiario>());
        _servico = new ClienteServico(_clientes, _beneficiarios);
    }

    [Fact]
    public async Task Incluir_CpfInvalido_RetornaErroSemGravar()
    {
        var resultado = await _servico.IncluirAsync(CriarCliente("123.456.789-00"));

        Assert.False(resultado.Sucesso);
        Assert.Equal("CPF inválido", resultado.Erro);
        await _clientes.DidNotReceive().IncluirAsync(Arg.Any<Cliente>());
    }

    [Fact]
    public async Task Incluir_CpfJaCadastrado_RetornaErroSemGravar()
    {
        _clientes.CpfJaCadastradoAsync("52998224725", 0).Returns(true);

        var resultado = await _servico.IncluirAsync(CriarCliente(CpfCliente));

        Assert.False(resultado.Sucesso);
        Assert.Equal("CPF já cadastrado", resultado.Erro);
        await _clientes.DidNotReceive().IncluirAsync(Arg.Any<Cliente>());
    }

    [Fact]
    public async Task Incluir_BeneficiarioComCpfInvalido_RetornaErro()
    {
        var cliente = CriarCliente(CpfCliente, CriarBeneficiario("000.000.000-00", "Maria"));

        var resultado = await _servico.IncluirAsync(cliente);

        Assert.False(resultado.Sucesso);
        Assert.Contains("CPF do beneficiário inválido", resultado.Erro);
    }

    [Fact]
    public async Task Incluir_BeneficiarioComNomeCurto_RetornaErro()
    {
        var cliente = CriarCliente(CpfCliente, CriarBeneficiario(CpfBeneficiarioA, " Jo "));

        var resultado = await _servico.IncluirAsync(cliente);

        Assert.False(resultado.Sucesso);
        Assert.Contains("mínimo 3 letras", resultado.Erro);
    }

    [Fact]
    public async Task Incluir_BeneficiariosComMesmoCpf_RetornaErro()
    {
        var cliente = CriarCliente(
            CpfCliente,
            CriarBeneficiario(CpfBeneficiarioA, "Maria"),
            CriarBeneficiario("11144477735", "Joana"));

        var resultado = await _servico.IncluirAsync(cliente);

        Assert.False(resultado.Sucesso);
        Assert.Equal("Não é permitido mais de um beneficiário com o mesmo CPF", resultado.Erro);
    }

    [Fact]
    public async Task Incluir_DadosValidos_GravaClienteEBeneficiariosSoComNumeros()
    {
        _clientes.IncluirAsync(Arg.Any<Cliente>()).Returns(10L);
        var cliente = CriarCliente(CpfCliente, CriarBeneficiario(CpfBeneficiarioA, "  Maria  "));

        var resultado = await _servico.IncluirAsync(cliente);

        Assert.True(resultado.Sucesso);
        await _clientes.Received(1).IncluirAsync(Arg.Is<Cliente>(c => c.Cpf == "52998224725"));
        await _beneficiarios.Received(1).IncluirAsync(Arg.Is<Beneficiario>(b =>
            b.Cpf == "11144477735" && b.Nome == "Maria" && b.IdCliente == 10));
    }

    [Fact]
    public async Task Alterar_SincronizaBeneficiarios()
    {
        _beneficiarios.ListarAsync(5).Returns(new List<Beneficiario>
        {
            new() { Id = 1, Cpf = "11144477735", Nome = "Maria", IdCliente = 5 },
            new() { Id = 2, Cpf = "39053344705", Nome = "Joana", IdCliente = 5 }
        });

        var cliente = CriarCliente(
            CpfCliente,
            new Beneficiario { Id = 1, Cpf = CpfBeneficiarioA, Nome = "Maria Silva" },
            CriarBeneficiario("529.982.247-25", "Carlos"));
        cliente.Id = 5;

        var resultado = await _servico.AlterarAsync(cliente);

        Assert.True(resultado.Sucesso);
        await _beneficiarios.Received(1).ExcluirAsync(2);
        await _beneficiarios.Received(1).AlterarAsync(Arg.Is<Beneficiario>(b => b.Id == 1 && b.Nome == "Maria Silva"));
        await _beneficiarios.Received(1).IncluirAsync(Arg.Is<Beneficiario>(b => b.Nome == "Carlos" && b.IdCliente == 5));
        await _clientes.Received(1).AlterarAsync(cliente);
    }

    [Fact]
    public async Task Alterar_CpfDeOutroCliente_RetornaErro()
    {
        _clientes.CpfJaCadastradoAsync("52998224725", 5).Returns(true);
        var cliente = CriarCliente(CpfCliente);
        cliente.Id = 5;

        var resultado = await _servico.AlterarAsync(cliente);

        Assert.False(resultado.Sucesso);
        await _clientes.DidNotReceive().AlterarAsync(Arg.Any<Cliente>());
    }

    private static Cliente CriarCliente(string cpf, params Beneficiario[] beneficiarios) => new()
    {
        Nome = "João",
        Sobrenome = "da Silva",
        Cpf = cpf,
        Nacionalidade = "brasileira",
        Cep = "01011-100",
        Estado = "SP",
        Cidade = "São Paulo",
        Logradouro = "Rua Boa Vista 253",
        Email = "joao@email.com",
        Telefone = "(11) 2020-3030",
        Beneficiarios = beneficiarios.ToList()
    };

    private static Beneficiario CriarBeneficiario(string cpf, string nome) => new() { Cpf = cpf, Nome = nome };
}
