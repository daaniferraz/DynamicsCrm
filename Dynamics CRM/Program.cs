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

            ImportarContaCrmTerceiro(CrmImport, CrmNew);

            Console.WriteLine("Fim de Execução");

        }

        #region Importar

        static void ImportarContaCrmTerceiro(CrmServiceClient CrmImport, CrmServiceClient CrmNew)

        {
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true'>
                            <entity name='account'>
                                <attribute name='drf_cpfcnpj' />

                                <attribute name='telephone1' />
                                <attribute name='name' />
                                <attribute name='emailaddress1' />
                                <attribute name='accountid' />

                                <order attribute='name' descending='false' />
                                <filter type='and'>

                                    <condition attribute='name' operator='not-null' distinct='true' />
                                    <condition attribute='drf_cpfcnpj' operator='not-null' distinct='true'/>

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

                entidade.Attributes.Add("name", item["name"].ToString());


                if (CrmImport.Retrieve("account", idImport, new ColumnSet("drf_cpfcnpj")).Attributes.Contains("drf_cpfcnpj"))
                {
                    
                    entidade.Attributes.Add("grp3_cpfcnpj", item["drf_cpfcnpj"].ToString());
                    
                }
                else
                    entidade.Attributes.Add("grp3_cpfcnpj", "".ToString());
                    
                if (CrmImport.Retrieve("account", idImport, new ColumnSet("emailaddress1")).Attributes.Contains("emailaddress1"))
                {
                    entidade.Attributes.Add("emailaddress1", item["emailaddress1"].ToString());
                }else
                entidade.Attributes.Add("emailaddress1", "".ToString());





                registro = conection.Create(entidade);
            }

        }

        #endregion

    }
}
