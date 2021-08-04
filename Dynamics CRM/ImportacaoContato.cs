using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace Dynamics_CRM
{
    class ImportacaoContato
    {
        public void ImportarConta(CrmServiceClient CrmImport)

        {
            CreateEntidade createEntidade = new CreateEntidade();

            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='contact'>
                             <attribute name='firstname' />
                             <attribute name='lastname' />
                             <attribute name='drf_cpf' />
                             <attribute name='jobtitle' />
                             <attribute name='mobilephone' />
                             <attribute name='drf_idade' />
                             <attribute name='address1_line1' />
                             <attribute name='address1_city' />
                             <attribute name='address1_stateorprovince' />   
                             <attribute name='emailaddress1' />
                             <attribute name='address1_postalcode' />
                             <attribute name='address1_country' />
                             </entity>
                            </fetch>";

            EntityCollection colecao = CrmImport.RetrieveMultiple(new FetchExpression(query));

            var conection = new ConexaoCrm().Obter();

            foreach (var item in colecao.Entities)
            {
                
                try
                {

                    var entidade = new Entity("contact");

                    var nome = item["firstname"].ToString();
                    var cpf = item["drf_cpf"].ToString();

                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='contact'>
                                   <attribute name='firstname' />
                                   <attribute name='grp3_cpf' />
                                    <order attribute='firstname' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='firstname' operator='eq' value= '{0}'/>
                                        <condition attribute='grp3_cpf' operator='eq' value= '{1}'/>                               
                                    </filter>
                                </entity>
                            </fetch>";
                    //teste
                    query2 = string.Format(query2, nome.ToString(), cpf.ToString());

                    EntityCollection col = conection.RetrieveMultiple(new FetchExpression(query2));


                    if (col.Entities.Count == 0)
                    {

                        Guid registro = new Guid();
                        createEntidade.CreateEntidades(item, "contact", conection, registro);

                    }
                }
                catch (Exception ex)
                {
                    var entidadeErro = new Entity("grp3_erroimportacao");

                    entidadeErro.Attributes.Add("grp3_nomeentidade", "Contato");
                    entidadeErro.Attributes.Add("grp3_errogerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                    conection.Create(entidadeErro);
                    Console.WriteLine("Erro gerado e gravado na tabela de erros.");
                }

            }
        }
        }
}
