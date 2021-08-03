using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics_CRM
{
    class CreateEntidade
    {
        public Guid CreateEntidades(Entity item, String nameEntityCreate, CrmServiceClient conectionTo, Guid registro)
        {            
            var entidade = new Entity(nameEntityCreate);

            Boolean createRecord = false;

            foreach (var attributesValidate in item.Attributes)
            {
                string checkValueField = attributesValidate.Key.ToString();

                string checkValueFieldFrom = checkValueField;

                if (item.Attributes.Contains(checkValueField))
                {
                    checkValueField = checkValueField.Replace("drf", "grp3");

                    switch (item[checkValueFieldFrom].GetType().Name)
                    {
                        case "String":
                            entidade.Attributes.Add(checkValueField, item[checkValueFieldFrom].ToString());
                            createRecord = true;
                        break;
                        case "Int32":
                            entidade.Attributes.Add(checkValueField, item[checkValueFieldFrom]);
                            createRecord = true;
                        break;                            
                        case "Money":
                            Money valorPedido = (Money)item[checkValueFieldFrom]; 
                            entidade.Attributes.Add(checkValueField, valorPedido);
                            createRecord = true;
                        break;
                        case "EntityReference":
                            break;
                        default:
                        //entidade.Attributes.Add(checkValueField, attributesValidate.Value);
                        entidade.Attributes.Add(checkValueField, item[checkValueFieldFrom].ToString());
                        createRecord = true;
                            break;
                    }
                }
            }

            if (createRecord)
            {
                registro = conectionTo.Create(entidade);
            }

            return registro;
        }
    }
}
