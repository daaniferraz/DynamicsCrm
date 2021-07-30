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
        public void ImportarConta(CrmServiceClient CrmImport)

        {
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='account'>
                             <attribute name='name' />
                             <attribute name='drf_cpfcnpj' />
                             <attribute name='telephone1' />
                             <attribute name='address1_line1' />
                             <attribute name='address1_postalcode' />
                             <attribute name='address1_city' />
                             <attribute name='address1_stateorprovince' />
                             <attribute name='address1_country' />
                             <attribute name='creditlimit' />
                             <attribute name='transactioncurrencyid' />
                             <attribute name='emailaddress1' />
                                   
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

                    //var idImport = item["accountid"];
                    var temp = item.Id;

                    

                    var nome = item["name"].ToString();
                    var cpf = item["drf_cpfcnpj"].ToString();
                    string tel = "telephone1";
                    //var add1 = item["address1_line1"].ToString();
                    //var post = item["address1_postalcode"].ToString();
                    //var city = item["address1_city"].ToString();
                    //var state = item["address1_stateorprovince"].ToString();
                    //var contry = item["address1_country"].ToString();
                    //var credt = item["creditlimit"].ToString();
                    //var money = item["transactioncurrencyid"].ToString();
                    //var email = item["emailaddress1"].ToString();



                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='account'>

                                   <attribute name='name' />
                                   <attribute name='grp3_cpfcnpj' />
                                   

                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='name' operator='eq' value= '{0}'/>
                                        <condition attribute='grp3_cpfcnpj' operator='eq' value= '{1}'/>                               
                                    </filter>
                                </entity>
                            </fetch>";
                    query2 = string.Format(query2, nome.ToString(), cpf.ToString());

                    EntityCollection col = conection.RetrieveMultiple(new FetchExpression(query2));
                   
                    if (col.Entities.Count == 0)
                    {
                       

                        entidade.Attributes.Add("name", item["name"].ToString());
                        entidade.Attributes.Add("grp3_cpfcnpj", item["drf_cpfcnpj"].ToString());
                      
                        if (item.Attributes.Contains(tel) )
                        {
                            entidade.Attributes.Add(tel, item[tel].ToString());
                        }
                        //else
                        //    entidade.Attributes.Add("telephone1","".ToString());
                        //entidade.Attributes.Add("websiteurl", item["websiteurl"].ToString());

                        entidade.Attributes.Add("address1_line1", item["address1_line1"].ToString());
                        entidade.Attributes.Add("address1_postalcode", item["address1_postalcode"].ToString());
                        entidade.Attributes.Add("address1_city", item["address1_city"].ToString());
                        entidade.Attributes.Add("address1_stateorprovince", item["address1_stateorprovince"].ToString());
                        entidade.Attributes.Add("address1_country", item["address1_country"].ToString());
                        //entidade.Attributes.Add("creditlimit", item["creditlimit"]);
                        //entidade.Attributes.Add("transactioncurrencyid", item["transactioncurrencyid"]);
                        entidade.Attributes.Add("emailaddress1", item["emailaddress1"].ToString());
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









            }

        }
    }
}
