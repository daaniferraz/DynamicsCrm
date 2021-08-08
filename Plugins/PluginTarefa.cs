using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Plugin
{
    public class PluginTarefa : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //Permite traçar o log dos passos do programa.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            //Fornece acesso ao contexto para o evento que executou o Plugin.
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            //Verifica se o contexto inclui os parâmetros esperados.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                //Interface que implementa a IOrganizationsService.
                IOrganizationServiceFactory serviceFactory =
    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                

                try
                {
                    //Atribui valor à entidade.
                    Entity registroTask = new Entity("task");

                    registroTask["subject"] = "Follow Up";
                    registroTask["description"] = "Entre em contato com o Lead para tentar concretizar a venda.";
                    registroTask["scheduledstart"] = (DateTime.Now.AddDays(7));
                    registroTask["scheduledend"] = DateTime.Now.AddDays(7);
                    registroTask["prioritycode"] = new OptionSetValue(2);
                    registroTask["actualdurationminutes"] = 20;
                    registroTask.Attributes.Add("regardingobjectid", entity.ToEntityReference());

               
                    tracingService.Trace("TarefaPlugin : Criando a tarefa.");
                    service.Create(registroTask);
                }

                
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("Um erro ocorreu neste plugin. Procure o responsável da área!", ex);
                }
                catch(Exception ex)
                {
                    tracingService.Trace("PluginTarefa: {0}", ex.ToString());
                    throw;
                }

            }
        }
    }
}
