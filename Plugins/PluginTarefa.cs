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

            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity grp3_clientepotenciallead = (Entity)context.InputParameters["Target"];

                try
                {
                    //Atribui valor à Entity, neste caso, a tabela task(tarefa).
                    Entity RegistroTask = new Entity("task");
                    
                    //Atribui valor aos campos da entidade tarefa.
                    RegistroTask.Attributes.Add("subject", "Follow Up");
                                                               //Descrição a mudar.
                    RegistroTask.Attributes.Add("description", "Entre em contato com o Lead para tentar concretizar a venda.");
                    
                    //Atribui falor à data de conclusão por meio do objeto DateTime.
                    RegistroTask.Attributes.Add("scheduledend", DateTime.Now.AddDays(7));

                    //Opção para preencher valor selecionável ***Criar ENUM baixa (0), normal (1), alta (2).
                    RegistroTask.Attributes.Add("prioritycode", new OptionSetValue(2));

                    //Atribui valor ao tempo de duração da tarefa (INT).
                    RegistroTask.Attributes.Add("actualdurationminutes", 20);

                    //Atribuindo um dono à tarefa para que ela não fique "órfã".
                    RegistroTask.Attributes.Add("regardingobjectid", grp3_clientepotenciallead.ToEntityReference());

                    Guid tarefaGuid = service.Create(RegistroTask);
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
