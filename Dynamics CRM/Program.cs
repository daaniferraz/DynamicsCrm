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

            //ImportacaoLeads importacaoLead = new ImportacaoLeads();
            // ImportarContaCrmTerceiro = new ImportacaoConta();
            //ImportacaoPedido importacaoPedido = new ImportacaoPedido();
            ImportacaoContato importacaoContato = new ImportacaoContato();

            //ImportarContaCrmTerceiro.ImportarConta(CrmImport);
            importacaoContato.ImportarConta(CrmImport);
            //importacaoLead.ImportarLeads(CrmImport);
            //importacaoPedido.ImportarPedido(CrmImport);

            //TesteErick(CrmNew);

            Console.WriteLine("Fim de Execução");

            return;

        }



        static void TesteErick(CrmServiceClient serviceProxy)
        {
            Coloring colors = new Coloring();
            Guid registro = new Guid();


            Console.WriteLine("\t:::Começando os testes de ativação de PLUGIN:::\t ");
            

            #region TesteEmContas
            var entidade1 = new Entity("account");
            var entidade2 = new Entity("account");
            

            

            try
            {
                entidade1.Attributes.Add("name", "Teste 1");
                entidade1.Attributes.Add("grp3_cpfcnpj", "11111111111111");
                registro = serviceProxy.Create(entidade1);
            }
            catch (Exception ex)
            {
                colors.ChangeColor(Coloring.Situation.Erro);
                Console.WriteLine("O CNPJ já existe na base de dados. Ignorando essa importação!");
                Console.WriteLine("Nome da conta ignorada: " + entidade1.Attributes["name"].ToString());
                Console.WriteLine("CPF: " + entidade1.Attributes["grp3_cpfcnpj"].ToString());
                colors.ChangeColor(Coloring.Situation.Normal);
            }

            registro = new Guid();

            colors.ChangeColor(Coloring.Situation.Espacamento);            
            colors.ChangeColor(Coloring.Situation.Normal);

            try
            {
                entidade2.Attributes.Add("name", "Teste 2");
                entidade2.Attributes.Add("grp3_cpfcnpj", "11111111111111");
                registro = serviceProxy.Create(entidade2);
            }
            catch (Exception ex)
            {
                colors.ChangeColor(Coloring.Situation.Erro);
                Console.WriteLine("O CNPJ já existe na base de dados. Ignorando essa importação!");
                Console.WriteLine("Nome da conta ignorada: " + entidade2.Attributes["name"].ToString());
                Console.WriteLine("CPF: " + entidade2.Attributes["grp3_cpfcnpj"].ToString());
                colors.ChangeColor(Coloring.Situation.Normal);
                
            }


            #endregion

            colors.ChangeColor(Coloring.Situation.Espacamento);
            colors.ChangeColor(Coloring.Situation.Normal);

            var entidadeContato1 = new Entity("contact");
            var entidadeContato2 = new Entity("contact");
            registro = new Guid();

            try
            {
                entidadeContato1.Attributes.Add("firstname", "Contato de Teste");
                entidadeContato1.Attributes.Add("lastname", "N 1");
                entidadeContato1.Attributes.Add("grp3_cpf", "11111111111");
                registro = serviceProxy.Create(entidadeContato1);
            }
            catch (Exception ex)
            {
                colors.ChangeColor(Coloring.Situation.Erro);
                Console.WriteLine("O CPF já existe na base de dados. Ignorando essa importação!");
                Console.WriteLine("Nome do contato ignorado: " + entidadeContato1.Attributes["firstname"].ToString() + " " + entidadeContato1.Attributes["lastname"].ToString());
                Console.WriteLine("CPF: " + entidadeContato1.Attributes["grp3_cpf"].ToString());
                colors.ChangeColor(Coloring.Situation.Normal);
            }

            registro = new Guid();

            colors.ChangeColor(Coloring.Situation.Espacamento);
            colors.ChangeColor(Coloring.Situation.Normal);

            try
            {
                entidadeContato2.Attributes.Add("firstname", "Contato de Teste");
                entidadeContato2.Attributes.Add("lastname", "N 2");
                entidadeContato2.Attributes.Add("grp3_cpf", "11111111111");
                registro = serviceProxy.Create(entidadeContato2);
            }
            catch (Exception ex)
            {
                colors.ChangeColor(Coloring.Situation.Erro);
                Console.WriteLine("O CPF já existe na base de dados. Ignorando essa importação!");
                Console.WriteLine("Nome do contato ignorado: " + entidadeContato2.Attributes["firstname"].ToString() + " " + entidadeContato2.Attributes["lastname"].ToString());
                colors.ChangeColor(Coloring.Situation.Normal);
            }

        }


    }
}
