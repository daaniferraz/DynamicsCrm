using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics_CRM
{
    class ValidateNullField
    {
        public void Validation(Entity item, Entity entidade,string variavel)
        {
            if (item.Attributes.Contains(variavel))
            {
                entidade.Attributes.Add(variavel, item[variavel].ToString());
            }

        }
        public void ValidationMoney(Entity item, Entity entidade, string variavel)
        {
            if (item.Attributes.Contains(variavel))
            {
                entidade.Attributes.Add(variavel, (Money)item[variavel]);
                               
            }

        }

    }
}
