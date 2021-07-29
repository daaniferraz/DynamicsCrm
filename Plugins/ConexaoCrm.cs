/*using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics_CRM
{
    class ConexaoCrm
    {
        private static CrmServiceClient crmServiceClientDestino;

        public CrmServiceClient Obter()
        {
            var connectionStringCRM = @"AuthType=OAuth;
            Username = grupo3verde@grupo3verde.onmicrosoft.com;
            Password = Grupo3@2021; SkipDiscovery = True;
            AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
            Url = https://org532fce75.crm2.dynamics.com/main.aspx;";


            if (crmServiceClientDestino == null)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                crmServiceClientDestino = new CrmServiceClient(connectionStringCRM);
            }
            return crmServiceClientDestino;
        }
    }
}
*/