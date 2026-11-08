using FI.AtividadeEntrevista.Application.Servicos;
using FI.AtividadeEntrevista.Domain.Validacoes;
using FI.AtividadeEntrevista.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FI.AtividadeEntrevista.Web.Controllers;

public class ClienteController : Controller
{
    private readonly IClienteServico _servico;

    public ClienteController(IClienteServico servico)
    {
        _servico = servico;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult Incluir() => View(new ClienteModel());

    [HttpPost]
    public async Task<IActionResult> Incluir([FromBody] ClienteModel model)
    {
        if (!ModelState.IsValid)
            return Erro(ErrosDoModelo());

        var resultado = await _servico.IncluirAsync(model.ParaEntidade());

        return resultado.Sucesso
            ? Ok(new { Mensagem = "Cadastro efetuado com sucesso" })
            : Erro(resultado.Erro!);
    }

    [HttpGet]
    public async Task<IActionResult> Alterar(long id)
    {
        var cliente = await _servico.ConsultarAsync(id);

        return cliente is null ? NotFound() : View(cliente.ParaModel());
    }

    [HttpPost]
    public async Task<IActionResult> Alterar([FromBody] ClienteModel model)
    {
        if (!ModelState.IsValid)
            return Erro(ErrosDoModelo());

        var resultado = await _servico.AlterarAsync(model.ParaEntidade());

        return resultado.Sucesso
            ? Ok(new { Mensagem = "Cadastro alterado com sucesso" })
            : Erro(resultado.Erro!);
    }

    [HttpGet]
    public async Task<IActionResult> ClienteList(int iniciarEm = 0, int quantidade = 5, string ordenacao = "Nome", bool crescente = true)
    {
        var pagina = await _servico.PesquisarAsync(iniciarEm, quantidade, ordenacao, crescente);

        var registros = pagina.Registros.Select(c => new
        {
            c.Id,
            c.Nome,
            Cpf = Cpf.Formatar(c.Cpf),
            c.Email
        });

        return Ok(new { Registros = registros, Total = pagina.Total });
    }

    private BadRequestObjectResult Erro(string mensagem) => BadRequest(new { Mensagem = mensagem });

    private string ErrosDoModelo()
    {
        var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

        return string.Join(Environment.NewLine, erros);
    }
}
