using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                string erro = ValidarDados(model);
                if (erro != null)
                {
                    Response.StatusCode = 400;
                    return Json(erro);
                }

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    CPF = CpfValidador.SomenteNumeros(model.CPF),
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                new BoBeneficiario().Gravar(model.Id, ConverterBeneficiarios(model));

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                string erro = ValidarDados(model);
                if (erro != null)
                {
                    Response.StatusCode = 400;
                    return Json(erro);
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    CPF = CpfValidador.SomenteNumeros(model.CPF),
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                new BoBeneficiario().Gravar(model.Id, ConverterBeneficiarios(model));

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    CPF = CpfValidador.Formatar(cliente.CPF),
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Beneficiarios = new List<BeneficiarioModel>()
                };

                foreach (Beneficiario ben in new BoBeneficiario().Listar(cliente.Id))
                {
                    model.Beneficiarios.Add(new BeneficiarioModel()
                    {
                        Id = ben.Id,
                        CPF = CpfValidador.Formatar(ben.CPF),
                        Nome = ben.Nome
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = (jtSorting ?? string.Empty).Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /// <summary>
        /// Valida o CPF do cliente, CPF duplicado e os beneficiários.
        /// </summary>
        private string ValidarDados(ClienteModel model)
        {
            if (!CpfValidador.EhValido(model.CPF))
                return "CPF inválido";

            if (new BoCliente().VerificarExistencia(CpfValidador.SomenteNumeros(model.CPF), model.Id))
                return "CPF já cadastrado";

            if (model.Beneficiarios != null)
            {
                foreach (BeneficiarioModel ben in model.Beneficiarios)
                {
                    if (!CpfValidador.EhValido(ben.CPF))
                        return "CPF do beneficiário inválido: " + ben.CPF;

                    if (ben.Nome.Trim().Length < 3)
                        return "O nome do beneficiário deve ter no mínimo 3 letras";
                }

                bool repetido = model.Beneficiarios
                    .GroupBy(b => CpfValidador.SomenteNumeros(b.CPF))
                    .Any(g => g.Count() > 1);

                if (repetido)
                    return "Não é permitido mais de um beneficiário com o mesmo CPF";
            }

            return null;
        }

        /// <summary>
        /// Converte os beneficiários da tela
        /// </summary>
        private List<Beneficiario> ConverterBeneficiarios(ClienteModel model)
        {
            List<Beneficiario> lista = new List<Beneficiario>();

            if (model.Beneficiarios != null)
            {
                foreach (BeneficiarioModel ben in model.Beneficiarios)
                {
                    lista.Add(new Beneficiario()
                    {
                        Id = ben.Id,
                        CPF = CpfValidador.SomenteNumeros(ben.CPF),
                        Nome = ben.Nome.Trim()
                    });
                }
            }

            return lista;
        }
    }
}