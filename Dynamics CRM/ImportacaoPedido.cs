using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace Dynamics_CRM
{
    class ImportacaoPedido
    {
        #region Pedido
        public void ImportarPedido(CrmServiceClient CrmImport)
        {
            CreateEntidade createEntidade = new CreateEntidade();
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='drf_pedidos'>                             
                             <attribute name='drf_idpedido' />
                             <attribute name='drf_idlead' />
                             <attribute name='drf_nomedocliente' />
                             <attribute name='drf_valordopedido' />                             
                             </entity>
                            </fetch>";

            EntityCollection colecaoFrom = CrmImport.RetrieveMultiple(new FetchExpression(query));

            var conectionTo = new ConexaoCrm().Obter();

            foreach (var item in colecaoFrom.Entities)
            {
                try
                {
                    //var entidade = new Entity("grp3_pedidos");

                    Guid registro = new Guid();

                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='grp3_pedidos'>                                        
                                        <attribute name='grp3_idpedido' />
                                        <attribute name='grp3_idlead' />
                                        <attribute name='grp3_nomedocliente' />
                                        <attribute name='grp3_valordopedido' />      
                                    <order attribute='grp3_idpedido' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_idpedido' operator='eq' value= '{0}'/>                                                                      
                                    </filter>
                                </entity>
                            </fetch>";

                    query2 = string.Format(query2, item["drf_idpedido"].ToString());

                    EntityCollection col = conectionTo.RetrieveMultiple(new FetchExpression(query2));

                    if (col.Entities.Count == 0)
                    {
                        //foreach (Entity entityValidate in colecaoFrom.Entities)
                        //{
                            registro = createEntidade.CreateEntidades(item, "grp3_pedidos", conectionTo, registro);
                        //}

                        //var validar = new ValidateNullField();

                        //Valida se exite valores e insere os campos com valores
                        /*foreach (Entity entityValidate in colecaoFrom.Entities)
                        {
                            foreach (var attributesValidate in entityValidate.Attributes)
                            {
                                string checkValueField = attributesValidate.Key.ToString();

                                string checkValueFieldFrom = checkValueField;

                                if (item.Attributes.Contains(checkValueField))
                                {
                                    checkValueField = checkValueField.Replace("drf", "grp3");

                                    switch (item[checkValueFieldFrom].GetType().Name)
                                    {
                                        case "String":
                                            entidade.Attributes.Add(checkValueField, attributesValidate.Value);// item[checkValueFieldFrom].ToString());
                                            break;
                                        case "Int32":
                                            entidade.Attributes.Add(checkValueField, attributesValidate.Value);// item[checkValueFieldFrom]);
                                            break;
                                        case "Money":
                                            //Money valorPedido = (Money)attributesValidate.Value;

                                            entidade.Attributes.Add(checkValueField, attributesValidate.Value);

                                            break;
                                        case "EntityReference":
                                            break;
                                        default:
                                            entidade.Attributes.Add(checkValueField, attributesValidate.Value);
                                            break;
                                    }
                                }
                            }
                        }

                        conectionTo.Create(entidade);*/
                    }
                }
                catch (Exception ex)
                {
                    var entidadeErro = new Entity("grp3_erroimportacao");

                    entidadeErro.Attributes.Add("grp3_nomeentidade", "Pedido");
                    entidadeErro.Attributes.Add("grp3_errogerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                    conectionTo.Create(entidadeErro);
                    Console.WriteLine("Erro gerado e gravado na tabela de erros.");
                }
            }
        }
        #endregion

        #region ItensPedido
        public void ImportarItensPedido(CrmServiceClient CrmImport)
        {
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='drf_itenspedidos'>
                                <attribute name='drf_idpedido' />
                                <attribute name='drf_iditem' />                                
                                <attribute name='drf_nomedoitem' />
                                <attribute name='drf_quantidade' />      
                                <attribute name='drf_unidadedevenda' />
                                <attribute name='drf_valorunitario' />
                                <attribute name='drf_valortotal' />                              
                             </entity>
                            </fetch>";

            EntityCollection colecaoFrom = CrmImport.RetrieveMultiple(new FetchExpression(query));

            var conectionTo = new ConexaoCrm().Obter();

            foreach (var item in colecaoFrom.Entities)
            {
                try
                {
                    var entidade = new Entity("grp3_itenspedidos");

                    //Guid registro = new Guid();

                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='grp3_itenspedidos'>
                                        <attribute name='grp3_idpedido' />
                                        <attribute name='grp3_iditem' />                                        
                                        <attribute name='grp3_nomeitem' />
                                        <attribute name='grp3_quantidade' />      
                                        <attribute name='grp3_unidadevenda' />
                                        <attribute name='grp3_valorunitario' />
                                        <attribute name='grp3_valorlinha' />  
                                    <order attribute='grp3_idpedido' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_idpedido' operator='eq' value= '{0}'/>                                                                      
                                    </filter>
                                </entity>
                            </fetch>";

                    query2 = string.Format(query2, item["drf_idpedido"].ToString());

                    EntityCollection col = conectionTo.RetrieveMultiple(new FetchExpression(query2));

                    if (col.Entities.Count == 0)
                    {
                        //valida se item existe na tabela produto e cria o item caso não tenha
                        this.CheckImportProdutos(conectionTo, item["grp3_iditem"].ToString(), item["grp3_nomeitem"].ToString());

                        //Valida se exite valores e insere os campos com valores
                        foreach (Entity entityValidate in colecaoFrom.Entities)
                        {
                            foreach (var attributesValidate in entityValidate.Attributes)
                            {
                                string checkValkueField = attributesValidate.Key.ToString();

                                if (item.Attributes.Contains(checkValkueField))
                                {
                                    //Console.WriteLine(attributesValidate.Value);
                                    entidade.Attributes.Add(checkValkueField, item[checkValkueField].ToString());
                                }
                            }
                        }

                        conectionTo.Create(entidade);
                        //registro = conectionTo.Create(entidade);
                    }
                }
                catch (Exception ex)
                {
                    var entidadeErro = new Entity("ErroImportacao");

                    entidadeErro.Attributes.Add("NomeEntidade", "ItensPedido");
                    entidadeErro.Attributes.Add("ErroGerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                    conectionTo.Create(entidadeErro);
                    Console.WriteLine("Erro Itens do pedido gerado e gravado na tabela de erros.");
                }
            }
        }
        #endregion

        #region Produtos
        public void CheckImportProdutos(CrmServiceClient CrmImportTo, string IdItem, string nomeItem)
        {
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='Produtos'>
                                <attribute name='grp3_iditem' />                                                    
                             </entity>
                            </fetch>";

            EntityCollection colecao = CrmImportTo.RetrieveMultiple(new FetchExpression(query));

            if (colecao.Entities.Count > 0)
            {
                return;
            }

            try
            {

                var entidadeProduto = new Entity("Prdutos");

                entidadeProduto.Attributes.Add("grp3_nomeitem", nomeItem);
                entidadeProduto.Attributes.Add("grp3_iditem", Convert.ToInt32(IdItem));

                CrmImportTo.Create(entidadeProduto);
                Console.WriteLine("Item criado na tabela Produtos: " + nomeItem);
            }
            catch (Exception ex)
            {
                var entidadeErro = new Entity("ErroImportacao");

                entidadeErro.Attributes.Add("NomeEntidade", "ItensPedido");
                entidadeErro.Attributes.Add("ErroGerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                CrmImportTo.Create(entidadeErro);
                Console.WriteLine("Erro Itens do pedido gerado e gravado na tabela de erros.");
            }
        }
        #endregion
    }
}
