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

            ImportarContaCrmTerceiro(CrmImport);

            Console.WriteLine("Fim de Execução");

        }

        #region Importar

        static void ImportarContaCrmTerceiro(CrmServiceClient CrmImport)

        {
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='false'>
                            <entity name='account'>
                                <attribute name='name' />
                                <attribute name='primarycontactid' />
                                <attribute name='telephone1' />
                                <attribute name='accountid' />
                                <attribute name='createdon' />
                                <attribute name='emailaddress1'/>
                                <order attribute='name' descending='false' />
                                <filter type='and'>
                                    <condition attribute='name' operator='not-null' />
                                    
                                </filter>
                            </entity>
                            </fetch>";
            EntityCollection colecao = CrmImport.RetrieveMultiple(new FetchExpression(query));
            var conection = new ConexaoCrm().Obter();

            foreach (var item in colecao.Entities)
            {
                var entidade = new Entity("account");
                
                Guid registro = new Guid();

                var idImport = item.Id;

                entidade.Attributes.Add("name",item["name"].ToString());

                
                if (CrmImport.Retrieve("account", idImport, new ColumnSet("telephone1")).Attributes.Contains("telephone1"))
                {
                    entidade.Attributes.Add("telephone1", item["telephone1"].ToString());
                }
                else
                    entidade.Attributes.Add("telephone1", "".ToString());

                //entidade.Attributes.Add("emailaddress1", item["emailaddress1"].ToString());
                //entidade.Attributes.Add("accountid", item["accountid"]);
                
                registro = conection.Create(entidade);
            }

        }

        #endregion

    }
}
