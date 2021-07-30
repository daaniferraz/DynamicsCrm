using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PluginStepValidation;

namespace AccountPreValidation
{
    public class Account_CnpjPreValidation : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName.ToLower() == "create" || context.MessageName.ToLower() == "update" && context.Mode == Convert.ToInt32(PluginStepEnum.Mode.Synchronous) &&
                context.Stage == Convert.ToInt32(PluginStepEnum.Stage.PreValidation))
            {

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                trace.Trace("Iniciando o Plugin para validar se o CNPJ está duplicado.");

                Entity CrmTargetEntity = null;

                if (context.InputParameters.Contains("Target"))
                {
                    CrmTargetEntity = (Entity)context.InputParameters["Target"];
                }


                trace.Trace("Entidade atribuida ao CrmTargetEntity");

                QueryExpression queryExpression = new QueryExpression("account");
                queryExpression.Criteria.AddCondition("grp3_cpfcnpj", ConditionOperator.Equal, CrmTargetEntity.Attributes["grp3_cpfcnpj"].ToString());
                queryExpression.ColumnSet = new ColumnSet("grp3_cpfcnpj");
                EntityCollection colecaoEntidades = service.RetrieveMultiple(queryExpression);


                if (colecaoEntidades.Entities.Count > 0)
                {
                    throw new InvalidPluginExecutionException("CNPJ já cadastrado. Por favor insira um CNPJ ainda não cadastrado.");
                }
            }

        }
        
    }
}
