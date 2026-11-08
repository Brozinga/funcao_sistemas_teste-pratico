using FI.AtividadeEntrevista.Domain.Entidades;
using FI.AtividadeEntrevista.Domain.Repositorios;
using FI.AtividadeEntrevista.Domain.Validacoes;

namespace FI.AtividadeEntrevista.Application.Servicos;

public class ClienteServico : IClienteServico
{
    private const int TamanhoMinimoNomeBeneficiario = 3;

    private readonly IClienteRepositorio _clientes;
    private readonly IBeneficiarioRepositorio _beneficiarios;

    public ClienteServico(IClienteRepositorio clientes, IBeneficiarioRepositorio beneficiarios)
    {
        _clientes = clientes;
        _beneficiarios = beneficiarios;
    }

    public async Task<Resultado> IncluirAsync(Cliente cliente)
    {
        var erro = await ValidarAsync(cliente);
        if (erro is not null)
            return Resultado.Falha(erro);

        Normalizar(cliente);

        var id = await _clientes.IncluirAsync(cliente);
        await GravarBeneficiariosAsync(id, cliente.Beneficiarios);

        return Resultado.Ok();
    }

    public async Task<Resultado> AlterarAsync(Cliente cliente)
    {
        var erro = await ValidarAsync(cliente);
        if (erro is not null)
            return Resultado.Falha(erro);

        Normalizar(cliente);

        await _clientes.AlterarAsync(cliente);
        await GravarBeneficiariosAsync(cliente.Id, cliente.Beneficiarios);

        return Resultado.Ok();
    }

    public async Task<Cliente?> ConsultarAsync(long id)
    {
        var cliente = await _clientes.ConsultarAsync(id);
        if (cliente is null)
            return null;

        cliente.Beneficiarios = (await _beneficiarios.ListarAsync(id)).ToList();

        return cliente;
    }

    public async Task<PaginaClientes> PesquisarAsync(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente)
    {
        var (registros, total) = await _clientes.PesquisarAsync(iniciarEm, quantidade, campoOrdenacao, crescente);

        return new PaginaClientes(registros, total);
    }

    private async Task<string?> ValidarAsync(Cliente cliente)
    {
        if (!Cpf.EhValido(cliente.Cpf))
            return "CPF inválido";

        if (await _clientes.CpfJaCadastradoAsync(Cpf.SomenteNumeros(cliente.Cpf), cliente.Id))
            return "CPF já cadastrado";

        return ValidarBeneficiarios(cliente.Beneficiarios);
    }

    private static string? ValidarBeneficiarios(List<Beneficiario> beneficiarios)
    {
        foreach (var beneficiario in beneficiarios)
        {
            if (!Cpf.EhValido(beneficiario.Cpf))
                return $"CPF do beneficiário inválido: {beneficiario.Cpf}";

            if (beneficiario.Nome.Trim().Length < TamanhoMinimoNomeBeneficiario)
                return $"O nome do beneficiário deve ter no mínimo {TamanhoMinimoNomeBeneficiario} letras";
        }

        var cpfRepetido = beneficiarios
            .GroupBy(b => Cpf.SomenteNumeros(b.Cpf))
            .Any(grupo => grupo.Count() > 1);

        return cpfRepetido ? "Não é permitido mais de um beneficiário com o mesmo CPF" : null;
    }

    private static void Normalizar(Cliente cliente)
    {
        cliente.Cpf = Cpf.SomenteNumeros(cliente.Cpf);

        foreach (var beneficiario in cliente.Beneficiarios)
        {
            beneficiario.Cpf = Cpf.SomenteNumeros(beneficiario.Cpf);
            beneficiario.Nome = beneficiario.Nome.Trim();
        }
    }

    private async Task GravarBeneficiariosAsync(long idCliente, List<Beneficiario> enviados)
    {
        var existentes = await _beneficiarios.ListarAsync(idCliente);

        foreach (var existente in existentes.Where(e => enviados.All(b => b.Id != e.Id)))
            await _beneficiarios.ExcluirAsync(existente.Id);

        foreach (var beneficiario in enviados)
        {
            beneficiario.IdCliente = idCliente;

            if (beneficiario.Id == 0)
                await _beneficiarios.IncluirAsync(beneficiario);
            else if (existentes.Any(e => e.Id == beneficiario.Id))
                await _beneficiarios.AlterarAsync(beneficiario);
        }
    }
}
