using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;


namespace Dynamics_CRM
{
    class ImportacaoLeads
    {
        public void ImportarLeads(CrmServiceClient CrmImport)

        {
            CreateEntidade createEntidade = new CreateEntidade();
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='drf_clientepotenciallead'>
                             <attribute name='drf_nome' />
                             <attribute name='drf_cpfoucnpj' />
                             <attribute name='drf_cpf' />
                             <attribute name='drf_cnpj' />
                             <attribute name='drf_cep' />
                             <attribute name='drf_endereco' />
                             <attribute name='drf_complemento' />
                             <attribute name='drf_bairro' />
                             <attribute name='drf_telefone' />
                             <attribute name='drf_telefone2' />
                             </entity>
                            </fetch>";

            EntityCollection colecao = CrmImport.RetrieveMultiple(new FetchExpression(query));
            var conectionTo = new ConexaoCrm().Obter();
            string nameEntity = "grp3_clientepotenciallead";

            foreach (var item in colecao.Entities)
            {
                try
                {
                    var entidade = new Entity("drf_clientepotenciallead");
                    string cpfCnpj;
                    string nameField;

                    var nome = item["drf_nome"].ToString();
                    
                    if (item.Attributes.Contains("drf_cpf"))
                    {
                        cpfCnpj = item["drf_cpf"].ToString();
                        nameField = "grp3_cpf";
                    }
                    else
                    {
                        cpfCnpj = item["drf_cnpj"].ToString();
                        nameField = "grp3_cnpj";
                    }

                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='grp3_clientepotenciallead'>
                                    <attribute name='grp3_nome'/>
                                    <attribute name='grp3_cpf'/>
                                    <attribute name='grp3_cnpj'/>
                                    <order attribute='grp3_nome' descending='false' />
                                      <filter type='and'>
                                        <condition attribute='grp3_nome' operator='eq' value= '{0}'/>
                                        <condition attribute='{1}' operator='eq' value= '{2}'/>
                                      </filter>                                                 
                                </entity>
                            </fetch>";
                    query2 = string.Format(query2, nome.ToString(),nameField,cpfCnpj.ToString());

                    EntityCollection col = conectionTo.RetrieveMultiple(new FetchExpression(query2));

                    if (col.Entities.Count == 0)
                    {
                        Guid registro = new Guid();
                        createEntidade.CreateEntidades(item, nameEntity, conectionTo, registro);
                    }
                }
                catch (Exception ex)
                {
                    Guid registro = new Guid();

                    createEntidade.CreateErrorException(ex, nameEntity, conectionTo, registro);
                }
            }
        }
    }
}