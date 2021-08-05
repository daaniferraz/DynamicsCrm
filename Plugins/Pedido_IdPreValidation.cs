using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PluginStepValidation;

namespace Plugin
{
    public class Pedido_IdPreValidation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName.ToLower() == "create" || context.MessageName.ToLower() == "update" &&
                context.Mode == Convert.ToInt32(PluginStepEnum.Mode.Synchronous) && 
                context.Stage == Convert.ToInt32(PluginStepEnum.Stage.PreValidation))
            {

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                trace.Trace("Iniciando o plugin para validar se o ID não é duplicado");

                Entity CrmTargetEntity = null;

                if (context.InputParameters.Contains("Target"))
                {
                    CrmTargetEntity = (Entity)context.InputParameters["Target"];
                }

                QueryExpression queryExpression = new QueryExpression("grp3_pedidos");
                queryExpression.Criteria.AddCondition("grp3_pedido", ConditionOperator.Equal, CrmTargetEntity.Attributes["grp3_pedido"].ToString());
                EntityCollection colecaoEntidades = service.RetrieveMultiple(queryExpression);

                if(colecaoEntidades.Entities.Count > 0)
                {
                    throw new InvalidPluginExecutionException("O ID do Pedido, já está cadastrado no sistema.");
                }
            }
            
        }
    }
}
