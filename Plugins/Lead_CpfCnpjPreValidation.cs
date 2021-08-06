using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
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

                try
                {
                    trace.Trace("Iniciando o Plugin para validar se o CPF está duplicado.");

                    Entity CrmTargetEntity = null;

                    if (context.InputParameters.Contains("Target"))
                    {
                        CrmTargetEntity = (Entity)context.InputParameters["Target"];
                    }


                    trace.Trace("Entidade atribuida ao CrmTargetEntity");

                    QueryExpression queryExpression = new QueryExpression("grp3_clientepotenciallead");

                    string campoDesejado = "";
                    string msgError = "";

                    if (((OptionSetValue)CrmTargetEntity["grp3_cpfoucnpj"]).Value == 100000000) //Checa se o campo é Pessoa Física.
                    {
                        campoDesejado = "grp3_cpf";
                        msgError = "CPF já cadastrado. Por favor insira um ainda não cadastrado.";
                    }
                    else if (((OptionSetValue)CrmTargetEntity["grp3_cpfoucnpj"]).Value == 100000001) //Checa se o campo é Pessoa Juridica
                    {
                        campoDesejado = "grp3_cnpj";
                        msgError = "CNPJ já cadastrado. Por favor insira um ainda não cadastrado.";
                    }


                    queryExpression.Criteria.AddCondition(campoDesejado, ConditionOperator.Equal, CrmTargetEntity.Attributes[campoDesejado].ToString());
                    queryExpression.ColumnSet = new ColumnSet(campoDesejado);
                    EntityCollection colecaoEntidades = service.RetrieveMultiple(queryExpression);


                    if (colecaoEntidades.Entities.Count > 0)
                    {
                        throw new InvalidPluginExecutionException(msgError);
                    }
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("Um erro ocorreu com o Plugin de validação de CPF/CNPJ Duplicado! Se possível contate o suporte. Ex: " + ex.ToString());
                }

                
                
            }

        }

    }
}