using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using PluginStepValidation;

namespace ContactPreValidation
{
    public class Contact_CpfPreValidation : IPlugin
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

                try
                {
                    trace.Trace("Iniciando o Plugin para validar se o CPF está duplicado.");

                    Entity CrmTargetEntity = null;

                    if (context.InputParameters.Contains("Target"))
                    {
                        CrmTargetEntity = (Entity)context.InputParameters["Target"];
                    }


                    trace.Trace("Entidade atribuida ao CrmTargetEntity");

                    QueryExpression queryExpression = new QueryExpression("contact");
                    queryExpression.Criteria.AddCondition("grp3_cpf", ConditionOperator.Equal, CrmTargetEntity.Attributes["grp3_cpf"].ToString());
                    queryExpression.ColumnSet = new ColumnSet("grp3_cpf");
                    EntityCollection colecaoEntidades = service.RetrieveMultiple(queryExpression);


                    if (colecaoEntidades.Entities.Count > 0)
                    {
                        throw new InvalidPluginExecutionException("CPF já cadastrado. Por favor insira um CPF ainda não cadastrado.");
                    }
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("Um erro ocorreu com o Plugin de validação de CPF Duplicado! Se possível contate o suporte. Ex: " + ex.ToString());
                }

               
            }

        }

    }
}