using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace Dynamics_CRM
{
    class ImportacaoLeads
    {
        public void ImportarConta(CrmServiceClient CrmImport)

        {
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='clientepotenciallead'>
                             <attribute name='name' />
                             <attribute name='drf_cpfcnpj' />
                             </entity>
                            </fetch>";

            EntityCollection colecao = CrmImport.RetrieveMultiple(new FetchExpression(query));
            var conection = new ConexaoCrm().Obter();

            foreach (var item in colecao.Entities)
            {
                try
                {
                    var entidade = new Entity("account");

                    Guid registro = new Guid();

                    var idImport = item.Id;

                    var nome = item["name"].ToString();
                    var cpf = item["drf_cpfcnpj"].ToString();



                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='clientepotenciallead'>
                                    <attribute name='name'/>
                                    <attribute name='grp3_cpf'/>
                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='name' operator='eq' value= '{0}'/>
                                        <condition attribute='grp3_cpf' operator='eq' value= '{1}'/>                               
                                    </filter>
                                </entity>
                            </fetch>";
                    query2 = string.Format(query2, nome.ToString(), cpf.ToString());

                    EntityCollection col = conection.RetrieveMultiple(new FetchExpression(query2));


                    if (col.Entities.Count == 0)
                    {
                        entidade.Attributes.Add("name", item["name"].ToString());
                        entidade.Attributes.Add("grp3_cpf", item["drf_cpfcnpj"].ToString());
                        registro = conection.Create(entidade);

                    }
                    else
                    {

                        throw new Exception();
                    }

                }
                catch (Exception)
                {

                    Console.WriteLine("Log");
                }



                //if (!(CrmImport.Retrieve("account", idImport, new ColumnSet("drf_cpfcnpj")).Attributes.Contains("drf_cpfcnpj")))
                //{

                //    entidade.Attributes.Add("grp3_cpfcnpj", "".ToString());

                //}
                //else
                //    entidade.Attributes.Add("grp3_cpfcnpj", item["drf_cpfcnpj"].ToString());






            }

        }
    }
}