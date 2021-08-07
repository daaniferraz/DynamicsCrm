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
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity grp3_clientepotenciallead = (Entity)context.InputParameters["Target"];

                IOrganizationServiceFactory serviceFactory =
    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    //Atribui valor à entidade.
                    Entity registroTask = new Entity("task");

                    registroTask["subject"] = "Follow up.";
                    registroTask["description"] = "Entre em contato com o Lead para tentar concretizar a venda.";
                    registroTask["scheduledend"] = DateTime.Now.AddDays(7);
                    registroTask["prioritycode"] = new OptionSetValue(2);
                    registroTask["actualdurationminutes"] = 20;

                    if (context.OutputParameters.Contains("grp3_nome"))
                    {
                        Guid regardingobjectid = new Guid(context.OutputParameters["grp3_nome"].ToString());
                        string regardingobjectidType = "grp3_clientepotenciallead";

                        registroTask["regardingobjectid"] =
                        new EntityReference(regardingobjectidType, regardingobjectid);
                    }

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
