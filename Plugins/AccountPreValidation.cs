using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Dynamics_CRM;

namespace AccountPreValidation
{
    public class AccountPreValidation : IPlugin
    {


        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var crmConnection = new ConexaoCrm().Obter();

            string cnpj = null;

            if (context.InputParameters.Contains("Target"))
            {
                cnpj = (string)context.InputParameters["Target"];
            }
            else
            {
                return;
            }

            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='account'>
                             <attribute name='drf_cpfcnpj' />
                             </entity>
                            </fetch>";

            EntityCollection colecao = crmConnection.RetrieveMultiple(new FetchExpression(query));
            bool duplicado = false;

            foreach (var item in colecao.Entities)
            {
                if(item["drf_cpfcnpj"].ToString() == cnpj)
                {
                    duplicado = true;
                    break; //Cancelar o foreach caso já encontre um item duplicado
                }
            }

            if (duplicado)
            {
                throw new InvalidPluginExecutionException("Cnpj já cadastrado no sistema!!");
            }
        }

    }
}
