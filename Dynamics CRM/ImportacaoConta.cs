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
    class ImportacaoConta
    {
        public  void ImportarConta(CrmServiceClient CrmImport)

        {
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical'>
                            <entity name='account'>
                             <attribute name='name'  />
                             <attribute name='drf_cpfcnpj' distinct='true'/>
                             <attribute name='accountid' />

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

                //if (CrmImport.Retrieve("account", idImport, new ColumnSet("emailaddress1")).Attributes.Contains("emailaddress1"))
                //{
                //    entidade.Attributes.Add("emailaddress1", item["emailaddress1"].ToString());
                //}else
                //entidade.Attributes.Add("emailaddress1", "".ToString());
                registro = conection.Create(entidade);

            }

        }
    }
}
