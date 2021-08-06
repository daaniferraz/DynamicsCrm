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
        private string fieldName;
        private Guid   fieldValue;
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
                    Console.WriteLine(item[checkValueFieldFrom].GetType().Name);
                    switch (item[checkValueFieldFrom].GetType().Name)
                    {
                        /*case "String":
                            //entidade.Attributes.Add(checkValueField, item[checkValueFieldFrom].ToString());
                            entidade.Attributes.Add(checkValueField, attributesValidate.Value);
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
                        case "OptionSetValue":
                            entidade.Attributes.Add(checkValueField, attributesValidate.Value);
                            createRecord = true;
                            break;*/
                        case "EntityReference":
                            if (checkValueField.ToString() == this.getfieldName && this.getfieldName.Length > 0)
                            {
                                EntityReference newEntityReference=  new EntityReference(this.getfieldName, this.getfieldValue);
                                entidade.Attributes.Add(checkValueField, newEntityReference);
                            }
                            break;
                        default:
                        entidade.Attributes.Add(checkValueField, attributesValidate.Value);
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

        public Guid CreateErrorException(Exception ex, string nameEntityCreate, CrmServiceClient conectionTo, Guid registro)
        {
            var entidadeErro = new Entity("grp3_erroimportacao");

            entidadeErro.Attributes.Add("grp3_nomeentidade", nameEntityCreate);
            entidadeErro.Attributes.Add("grp3_errogerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

            conectionTo.Create(entidadeErro);
            Console.WriteLine("Erro gerado e gravado na tabela de erros.");

            return registro;
        }

        public String getfieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        public Guid getfieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
    }
}
