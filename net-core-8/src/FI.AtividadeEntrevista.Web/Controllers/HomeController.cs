using System.Diagnostics;
using FI.AtividadeEntrevista.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FI.AtividadeEntrevista.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErroModel { IdRequisicao = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
