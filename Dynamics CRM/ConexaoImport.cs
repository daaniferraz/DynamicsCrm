using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Xrm.Tooling.Connector;

namespace Dynamics_CRM
{
    class ConexaoImport
    {
        private static CrmServiceClient crmServiceClientDestino;

        public CrmServiceClient Obter()
        {
            var connectionStringCRM = @"AuthType=OAuth;
            Username = grupo2legado@danielrodriguesferraz.onmicrosoft.com;
            Password = Grupo2d#ness; SkipDiscovery = True;
            AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
            Url = https://org44041492.crm2.dynamics.com/main.aspx;";


            if (crmServiceClientDestino == null)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                crmServiceClientDestino = new CrmServiceClient(connectionStringCRM);
            }
            return crmServiceClientDestino;
        }
    }
}
