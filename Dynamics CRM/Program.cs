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

            ImportacaoLeads importacaoLead = new ImportacaoLeads();
            ImportacaoConta ImportarContaCrmTerceiro = new ImportacaoConta();
            ImportacaoPedido importacaoPedido = new ImportacaoPedido();
            ImportacaoContato importacaoContato = new ImportacaoContato();

            ImportarContaCrmTerceiro.ImportarConta(CrmImport);
            Console.WriteLine("Contas criadas com sucesso!");

            importacaoContato.ImportarContato(CrmImport);
            Console.WriteLine("Contatos criados com sucesso!");

            importacaoLead.ImportarLeads(CrmImport);
            Console.WriteLine("Leads criados com sucesso!");

            importacaoPedido.ImportarPedido(CrmImport);
            Console.WriteLine("Pedidos criados com sucesso!");   
            
            importacaoPedido.ImportarItensPedido(CrmImport);
            Console.WriteLine("Itens dos Pedidos criados com sucesso!");

            Console.WriteLine("Fim das importações.");
            Console.Read();

            return;

        }

    }
}
