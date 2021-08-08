using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;


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
            string nameEntity = "account";

            foreach (var item in colecao.Entities)
            {
                var nameError = item["name"].ToString();

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
                    //teste
                    query2 = string.Format(query2, nome.ToString(), cpf.ToString());

                    EntityCollection col = conection.RetrieveMultiple(new FetchExpression(query2));


                    if (col.Entities.Count == 0)
                    {

                        Guid registro = new Guid();
                        createEntidade.CreateEntidades(item, nameEntity, conection, registro);

                    }
                }
                catch (Exception ex)
                {
                    Guid registro = new Guid();

                    Console.WriteLine("Não foi possível importar a conta: " + nameError);
                    createEntidade.CreateErrorException(ex, nameEntity, conection, registro);
                }

            }

        }
    }
}
