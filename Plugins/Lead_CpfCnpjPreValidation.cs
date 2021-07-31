using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PluginStepValidation;

namespace LeadPreValidation
{
    public class Lead_CpfCnpjPreValidation : IPlugin
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

                trace.Trace("Iniciando o Plugin para validar se o CPF está duplicado.");

                Entity CrmTargetEntity = null;

                if (context.InputParameters.Contains("Target"))
                {
                    CrmTargetEntity = (Entity)context.InputParameters["Target"];
                }


                trace.Trace("Entidade atribuida ao CrmTargetEntity");

                QueryExpression queryExpression = new QueryExpression("grp3_clientepotenciallead");

                if (CrmTargetEntity.Attributes["grp3_cpfoucnpj"] == new OptionSetValue(1))
                {
                    throw new InvalidPluginExecutionException("Pessoa física selecionada.");
                }
                else if (CrmTargetEntity.Attributes["grp3_cpfoucnpj"] == new OptionSetValue(2))
                {
                    throw new InvalidPluginExecutionException("Pessoa Juridica");
                }

                /*
                queryExpression.Criteria.AddCondition("grp3_cpf", ConditionOperator.Equal, CrmTargetEntity.Attributes["grp3_cpf"].ToString());
                queryExpression.ColumnSet = new ColumnSet("grp3_cpf");
                EntityCollection colecaoEntidades = service.RetrieveMultiple(queryExpression);


                if (colecaoEntidades.Entities.Count > 0)
                {
                    throw new InvalidPluginExecutionException("CPF já cadastrado. Por favor insira um CPF ainda não cadastrado.");
                }
                */
            }

        }

    }
}