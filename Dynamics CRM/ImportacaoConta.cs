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
            CreateEntidade createEntidade = new CreateEntidade();
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
                             <attribute name='emailaddress1' />
                             <attribute name='creditlimit' />
                             </entity>
                            </fetch>";

            EntityCollection colecao = CrmImport.RetrieveMultiple(new FetchExpression(query));
            
            var conection = new ConexaoCrm().Obter();

            foreach (var item in colecao.Entities)
            {
                //ntity entityValidate = item.Attribute;
                try
                {
                    
                    var entidade = new Entity("account");

                    var nome = item["name"].ToString();
                    var cpf = item["drf_cpfcnpj"].ToString();

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

                    string tel = "telephone1";
                    string add1 = "address1_line1";
                    string post = "address1_postalcode";
                    string city = "address1_city";
                    string state = "address1_stateorprovince";
                    string contry = "address1_country";
                    string email = "emailaddress1";
                    string cred = "creditlimit";

                    if (col.Entities.Count == 0)
                    {
                        //foreach (Entity entityValidate colecao.Entities)
                        //{
                            Guid registro = new Guid();
                            createEntidade.CreateEntidades(item, "account", conection, registro);
                        //}
                        /*var validar = new ValidateNullField();

                        entidade.Attributes.Add("name", item["name"].ToString());
                        entidade.Attributes.Add("grp3_cpfcnpj", item["drf_cpfcnpj"].ToString());

                        validar.Validation(item, entidade, tel);
                        validar.Validation(item, entidade, add1);
                        validar.Validation(item, entidade, post);
                        validar.Validation(item, entidade, city);
                        validar.Validation(item, entidade, state);
                        validar.Validation(item, entidade, contry);
                        validar.Validation(item, entidade, email);
                        validar.ValidationMoney(item, entidade, cred);
                        
                        registro = conection.Create(entidade);*/

                    }
                }
                catch (Exception ex)
                {
                    var entidadeErro = new Entity("grp3_erroimportacao");

                    entidadeErro.Attributes.Add("grp3_nomeentidade", "Conta");
                    entidadeErro.Attributes.Add("grp3_errogerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                    conection.Create(entidadeErro);
                    Console.WriteLine("Erro gerado e gravado na tabela de erros.");
                }

            }

        }
    }
}
