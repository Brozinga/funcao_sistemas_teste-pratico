using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Alterar(beneficiario);
        }

        /// <summary>
        /// Excluir o beneficiário pelo id
        /// </summary>
        /// <param name="id">id do beneficiário</param>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiários de um cliente
        /// </summary>
        /// <param name="idCliente">id do cliente</param>
        public List<DML.Beneficiario> Listar(long idCliente)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Listar(idCliente);
        }

        /// <summary>
        /// Grava os beneficiários que vieram da tela quando o cliente é salvo
        /// </summary>
        /// <param name="idCliente">id do cliente</param>
        /// <param name="beneficiarios">lista que veio da tela</param>
        public void Gravar(long idCliente, List<DML.Beneficiario> beneficiarios)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            List<DML.Beneficiario> noBanco = ben.Listar(idCliente);

            foreach (DML.Beneficiario atual in noBanco)
            {
                if (!beneficiarios.Any(b => b.Id == atual.Id))
                    ben.Excluir(atual.Id);
            }

           foreach (DML.Beneficiario b in beneficiarios)
            {
                b.IdCliente = idCliente;

                if (b.Id == 0)
                    ben.Incluir(b);
                else if (noBanco.Any(atual => atual.Id == b.Id))
                    ben.Alterar(b);
            }
        }
    }
}
