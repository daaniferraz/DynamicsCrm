using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PluginStepValidation;

namespace Plugin
{
    public class Pedido_IdPreValidation : IPlugin // Adquirindo a interface para trabalhar como Plugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //Variavel de contexto, usado para saber "onde" o plugin está sendo chamado.
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            //Validação se o Plugin está sendo chamado dentro da etapa correta. Caso não, nada será feito.
            //Verifica se é uma chamada de criação ou de update, logo em seguida valida se está sendo
            //chamado de forma Sincrona e para finalizar precisa checar se é uma PreValidation que está sendo chamada.
            if (context.MessageName.ToLower() == "create" || context.MessageName.ToLower() == "update" &&
                context.Mode == Convert.ToInt32(PluginStepEnum.Mode.Synchronous) && 
                context.Stage == Convert.ToInt32(PluginStepEnum.Stage.PreValidation))
            {
                //Criando a conexão com o serviso do CRM, com essa ligação é possivela acessar os dados já cadastrados no sistema
                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);

                //Usado para conseguir criar mensagens personalizadas no log do plugin, caso de algum erro, é possivel
                //saber o que já foi executado do plugin se usado essa função como debug.
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                trace.Trace("Iniciando o plugin para validar se o ID não é duplicado");

                //Criando uma entidade que vai ser usada para receber os dados do power apps ao clicar em salvar.
                Entity CrmTargetEntity = null;

                //Verifica se existe um "Target" previamente definido no XRMToolBox
                if (context.InputParameters.Contains("Target"))
                {
                    //Caso exista, atribuir a entidade desse "Target" para a variavel de entidade que criamos logo a cima.
                    CrmTargetEntity = (Entity)context.InputParameters["Target"];
                }

                //Criamos uma Query do tipo que estamos tratando no Plugin
                QueryExpression queryExpression = new QueryExpression("grp3_pedidos");
                //E adicionamos um critério de filtro para a mesma, para definir o que ela irá carregar.
                queryExpression.Criteria.AddCondition("grp3_pedido", ConditionOperator.Equal, CrmTargetEntity.Attributes["grp3_pedido"].ToString());
                //Mandamos então buscar as entidades no sistema que atentam o critério pré definido a cima e então adicionamos em uma coleção de entidades.
                EntityCollection colecaoEntidades = service.RetrieveMultiple(queryExpression);


                //Caso seja retornado alguma entidade na condição a cima, quer dizer que o dado que estamos validando
                //já está cadastrado no sistema, entrando assim então na condição a baixo e impedindo o registro duplicado.
                if(colecaoEntidades.Entities.Count > 0)
                {
                    //Joga uma exceção que irá impedir o dado de ser salvo, junto com a mensagem de erro do motivo.
                    throw new InvalidPluginExecutionException("O ID do Pedido, já está cadastrado no sistema.");
                }
            }
            
        }
    }
}
