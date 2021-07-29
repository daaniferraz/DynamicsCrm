using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
//using Dynamics_CRM;

namespace AccountPreValidation
{
    public class AccountPreValidation : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            trace.Trace("Comecando");
            //var crmConnection = new ConexaoCrm().Obter();
            
            Entity cnpj = null;


            trace.Trace("antes do if");
            if (context.InputParameters.Contains("Target"))
            {
                cnpj = (Entity)context.InputParameters["Target"];
            }
            else
            {
                return;
            }
            

            QueryExpression queryExpression = new QueryExpression("account");
            queryExpression.Criteria.AddCondition("grp3_cpfcnpj", ConditionOperator.Equal, cnpj);
            queryExpression.ColumnSet = new ColumnSet("grp3_cpfcnpj");
            EntityCollection colecaoEntidades = service.RetrieveMultiple(queryExpression);

            if(colecaoEntidades.Entities.Count > 0)
            {
                throw new InvalidPluginExecutionException("Cnpj já cadastrado no sistema!!");
            }

            /*
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='account'>
                             <attribute name='grp3_cpfcnpj' />
                             </entity>
                            </fetch>";

            
            QueryExpression leituraDados = new QueryExpression()
            {
                EntityName = "account",
                ColumnSet = new ColumnSet("grp3_cpfcnpj")
            };

            leituraDados.Criteria.AddCondition("grp3_cpfcnpj", ConditionOperator.Equal, cnpj);
            
            EntityCollection colecao = service.RetrieveMultiple(new FetchExpression(query));
            bool duplicado = false;


            foreach (var item in colecao.Entities)
            {
                if(item["grp3_cpfcnpj"].ToString() == cnpj.ToString())
                {
                    duplicado = true;
                    break; //Cancelar o foreach caso já encontre um item duplicado
                }
            }
            
            if (!duplicado)
            {
                throw new InvalidPluginExecutionException("Cnpj já cadastrado no sistema!!");
            }*/
        }
        
        }
    }
