using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dynamics_CRM
{
    class Program
    {
        static void Main(string[] args)
        {
            var CrmImport = new ConexaoImport().Obter();
            var CrmNew = new ConexaoCrm().Obter();
            var ImportarContaCrmTerceiro = new ImportacaoConta();
            ImportacaoPedido importacaoPedido = new ImportacaoPedido();

            ImportarContaCrmTerceiro.ImportarConta(CrmImport);
            importacaoPedido.ImportarPedido(CrmImport);


            Console.WriteLine("Fim de Execução");

        }

       
    }
}
