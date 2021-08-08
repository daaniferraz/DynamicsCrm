using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace Dynamics_CRM
{
    class ImportacaoContato
    {
        public void ImportarContato(CrmServiceClient CrmImport)

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
            string nameEntity = "contact";

            foreach (var item in colecao.Entities)
            {
                var nameError = item["firstname"].ToString();

                try
                {

                    var entidade = new Entity("contact");

                    var nome = item["firstname"].ToString();
                    var cpf = item["drf_cpf"].ToString();

                    string queryContact = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
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

                    queryContact = string.Format(queryContact, nome.ToString(), cpf.ToString());

                    EntityCollection col = conection.RetrieveMultiple(new FetchExpression(queryContact));


                    if (col.Entities.Count == 0)
                    {

                        Guid registro = new Guid();
                        createEntidade.CreateEntidades(item, nameEntity, conection, registro);

                    }
                }
                catch (Exception ex)
                {
                    Guid registro = new Guid();

                    Console.WriteLine("Não foi possível importar o contato: " + nameError);
                    createEntidade.CreateErrorException(ex, nameEntity, conection, registro);                    
                }

            }
        }
        }
}
